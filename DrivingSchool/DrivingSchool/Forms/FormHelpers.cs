using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities.Enums;
using Npgsql;

namespace DrivingSchool.Forms
{
    internal static class FormHelpers
    {
        internal static bool Run(Form owner, Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(owner, ex.Message, "Проверьте данные", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                var message = ex.GetBaseException() is PostgresException postgres ? postgres.SqlState switch
                {
                    PostgresErrorCodes.ForeignKeyViolation => "Связанная запись была удалена или используется в другой таблице.",
                    PostgresErrorCodes.UniqueViolation => "Такая запись уже существует.",
                    _ => "Не удалось выполнить операцию с базой данных. Проверьте журнал приложения."
                } : ex is NpgsqlException or TimeoutException
                    ? "Не удалось подключиться к базе данных. Проверьте настройки подключения и запуск PostgreSQL."
                    : ex.Message;
                MessageBox.Show(owner, message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        internal static T ShowSelection<T>(Form owner, DataGridView grid) where T : class
        {
            if (grid.CurrentRow?.DataBoundItem is T item)
                return item;
            MessageBox.Show(owner, "Выберите строку в списке.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        internal static DataGridView ConfigureList(Form form, string title, (string Property, string Caption, string Format)[] columns,
            params (string Caption, Action Click)[] actions)
        {
            form.Text = title;
            form.ClientSize = new Size(900, 450);
            form.MinimumSize = new Size(600, 300);
            form.StartPosition = FormStartPosition.CenterParent;
            var strip = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4) };
            foreach (var (caption, action) in actions)
            {
                var button = new Button { Text = caption, AutoSize = true };
                button.Click += (_, _) => Run(form, action);
                strip.Controls.Add(button);
            }
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill, AutoGenerateColumns = false, AllowUserToAddRows = false,
                AllowUserToDeleteRows = false, ReadOnly = true, MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            foreach (var (property, caption, format) in columns)
            {
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = caption, DataPropertyName = property,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = format }
                });
            }
            form.Controls.Add(grid);
            form.Controls.Add(strip);
            return grid;
        }

        internal static void ConfigureEditor(Form form, string title, Action save, params (string Caption, Control Input)[] fields)
        {
            form.Text = title;
            form.StartPosition = FormStartPosition.CenterParent;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AutoSize = false;
            form.AutoScroll = true;
            var table = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = fields.Length + 1,
                Location = Point.Empty,
                Padding = new Padding(12)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 325));
            for (var i = 0; i < fields.Length; i++)
            {
                var (caption, input) = fields[i];
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                table.Controls.Add(new Label { Text = caption, AutoSize = true, Anchor = AnchorStyles.Left }, 0, i);
                input.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                table.Controls.Add(input, 1, i);
            }
            var buttons = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.Right, WrapContents = false };
            var ok = new Button { Text = "Сохранить", AutoSize = true };
            var cancel = new Button { Text = "Отмена", AutoSize = true, DialogResult = DialogResult.Cancel };
            ok.Click += (_, _) =>
            {
                if (Run(form, save))
                    form.DialogResult = DialogResult.OK;
            };
            buttons.Controls.AddRange(new Control[] { ok, cancel });
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.Controls.Add(buttons, 1, fields.Length);
            form.Controls.Add(table);
            table.PerformLayout();
            var preferred = table.GetPreferredSize(Size.Empty);
            var workArea = Screen.FromControl(form).WorkingArea;
            form.ClientSize = new Size(
                Math.Min(Math.Max(520, preferred.Width), workArea.Width - 32),
                Math.Min(preferred.Height, workArea.Height - 64));
            form.AcceptButton = ok;
            form.CancelButton = cancel;
        }

        internal static ComboBox Choice(IEnumerable<(int Id, string Caption)> choices, int? selectedId = null)
        {
            var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            BindChoices(combo, choices, selectedId);
            return combo;
        }

        internal static void BindChoices(ComboBox combo, IEnumerable<(int Id, string Caption)> choices, int? selectedId = null)
        {
            combo.DataSource = null;
            combo.DisplayMember = nameof(ChoiceItem.Caption);
            combo.ValueMember = nameof(ChoiceItem.Id);
            var items = choices.Select(c => new ChoiceItem(c.Id, c.Caption)).ToList();
            var selectedIndex = selectedId.HasValue ? items.FindIndex(c => c.Id == selectedId.Value) : -1;
            combo.BindingContext = new BindingContext();
            combo.DataSource = items;
            combo.SelectedIndex = selectedIndex;
        }

        internal static int RequiredChoice(ComboBox combo, string message) =>
            combo.SelectedItem is ChoiceItem item ? item.Id : throw new ArgumentException(message);

        internal static string RequiredText(TextBox box, string message) =>
            !string.IsNullOrWhiteSpace(box.Text) ? box.Text.Trim() : throw new ArgumentException(message);

        internal static decimal Amount(TextBox box, bool allowZero, string message)
        {
            if (!decimal.TryParse(box.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var value) ||
                value < 0 || (!allowZero && value == 0) || value > 9_999_999_999.99m || decimal.Round(value, 2) != value)
                throw new ArgumentException(message);
            return value;
        }

        internal static DateTimePicker DatePicker(DateTime? value = null) =>
            new DateTimePicker { Format = DateTimePickerFormat.Short, Value = value ?? DateTime.Today };

        internal static IEnumerable<(TrainingDaysEnum Flag, string Caption)> TrainingDays => new[]
        {
            (TrainingDaysEnum.Monday, "Понедельник"), (TrainingDaysEnum.Tuesday, "Вторник"),
            (TrainingDaysEnum.Wednesday, "Среда"), (TrainingDaysEnum.Thursday, "Четверг"),
            (TrainingDaysEnum.Friday, "Пятница"), (TrainingDaysEnum.Saturday, "Суббота"),
            (TrainingDaysEnum.Sunday, "Воскресенье")
        };

        internal static string FormatTrainingDays(TrainingDaysEnum value) => string.Join(", ",
            TrainingDays.Where(day => value.HasFlag(day.Flag)).Select(day => day.Caption));

        private sealed record ChoiceItem(int Id, string Caption);
    }
}
