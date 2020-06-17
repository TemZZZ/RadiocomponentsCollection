﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Model;
using Model.PassiveComponents;


namespace View
{
	/// <summary>
	/// Элемент отбражения информации об уже созданном радиокомпоненте или
	/// предоставляющий информацию для создания нового радиокомпонента
	/// </summary>
	public partial class RadioComponentControl : UserControl
	{
		private bool _readOnly;
		private List<RadioButton> _radioButtons;

		/// <summary>
		/// Позволяет сделать элемент доступным только для чтения или
		/// узнать, доступен элемент только для чтения или нет
		/// </summary>
		public bool ReadOnly
		{
			get => _readOnly;
			
			set
			{
				_readOnly = value;
				valueDoubleTextBox.ReadOnly = _readOnly;

				foreach (var radioButton in _radioButtons)
				{
					radioButton.Enabled = !_readOnly;
				}
			}
		}

		/// <summary>
		/// Создает и возвращает радиокомпонент на основе присутствующей в
		/// полях элемента информации
		/// </summary>
		public RadioComponentBase RadioComponent
		{
			get
			{
				var radioComponentValue = valueDoubleTextBox.GetValue();
				var radioComponentfactory = new RadioComponentFactory();

				for (int i = 0; i < _radioButtons.Count; ++i)
				{
					if (_radioButtons[i].Checked)
					{
						return radioComponentfactory.CreateRadioComponent(
							(RadioComponentType)i, radioComponentValue);
					}
				}

				return null;
			}

			set
			{
				if (value is null)
				{
					SetDefaultState();
					return;
				}

				valueDoubleTextBox.Text = value.Value.ToString();
				
				quantityUnitLabel.Text
					= string.Join(", ", value.Quantity, value.Unit);

				var typeToRadioButtonMap
					= new Dictionary<Type, RadioButton>
					{
						[typeof(Resistor)] = resistorRadioButton,
						[typeof(Inductor)] = inductorRadioButton,
						[typeof(Capacitor)] = capacitorRadioButton
					};

				typeToRadioButtonMap[value.GetType()].Checked = true;
			}
		}

		/// <summary>
		/// Создает элемент отбражения информации об уже созданном
		/// радиокомпоненте или предоставляющий информацию для создания
		/// нового радиокомпонента
		/// </summary>
		public RadioComponentControl()
		{
			InitializeComponent();

			_radioButtons = new List<RadioButton>
			{
				resistorRadioButton,
				inductorRadioButton,
				capacitorRadioButton
			};

			foreach (var radioButton in _radioButtons)
			{
				radioButton.CheckedChanged += RadioButton_CheckedChanged;
			}

			SetDefaultState();
		}

		/// <summary>
		/// Устанавливает состояние элемента по умолчанию
		/// </summary>
		private void SetDefaultState()
		{
			const string defaultValueText = "0";

			valueDoubleTextBox.Text = defaultValueText;
			quantityUnitLabel.Text = string.Empty;

			foreach (var radioButton in _radioButtons)
			{
				radioButton.Checked = false;
			}
		}

		/// <summary>
		/// Изменяет текст <see cref="quantityUnitLabel"/>
		/// в зависимости от выбранной радиокнопки:
		/// <see cref="resistorRadioButton"/>
		/// <see cref="inductorRadioButton"/> или
		/// <see cref="capacitorRadioButton"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RadioButton_CheckedChanged(
			object sender, EventArgs e)
		{
			if (!(sender is RadioButton selectedRadioButton))
				return;

			var radioButtonToQuantityUnitTextMap
				= new Dictionary<RadioButton, string>
				{
					[resistorRadioButton] = "Сопротивление, Ом",
					[inductorRadioButton] = "Индуктивность, Гн",
					[capacitorRadioButton] = "Емкость, Ф"
				};

			quantityUnitLabel.Text
				= radioButtonToQuantityUnitTextMap[selectedRadioButton];
		}
	}
}
