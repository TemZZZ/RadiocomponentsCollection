﻿#define IS_RANDOM_BUTTON_VISIBLE

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Model;

namespace MVVM
{
    internal class AddRadiocomponentViewModel : ViewModelBase
    {
        #region -- Private fields --

        private List<RadiocomponentType> _availableRadiocomponentTypes;
        private ICollection<RadiocomponentToPrintableRadiocomponentAdapter>
            _radiocomponents;
        private bool _isRadiocomponentValueValid;
        private double _radiocomponentValue;

        // Эти поля в коде не трогать! Используй публичные свойства!
        private string _radiocomponentValueAsString = "0";
        private int? _selectedRadiocomponentTypeIndex;
        private RelayCommand _adddRadiocomponentCommand;
        private RelayCommand _setRandomRadiocomponentPropertiesCommand;

        #endregion

        #region -- Private methods --

        /// <summary>
        /// Проверяет, представляет ли строковое представление физической
        /// величины радиокомпонента неотрицательное вещественное число. Если
        /// да, то в true устанавливается соответствующий флаг, и обновляется
        /// значение поля, хранящего значение физической величины
        /// радиокомпонента.
        /// </summary>
        private void ValidateAndUpdateRadiocomponentValue()
        {
            _isRadiocomponentValueValid = NotNegativeDoubleValidationRule
                .TryConvertToNotNegativeDouble(_radiocomponentValueAsString,
                    out var newRadiocomponentValue);
            if (_isRadiocomponentValueValid)
            {
                _radiocomponentValue = newRadiocomponentValue;
            }
        }

        /// <summary>
        /// Включает радокнопку, соответствующую типу случайного
        /// радиокомпонента, а также вносит в текстовое поле значение
        /// физической величины этого случайного радиокомпонента.
        /// </summary>
        private void SetRandomRadiocomponentProperties()
        {
            var radiocomponent = RadiocomponentFactory
                .CreateRandomRadiocomponent();
            var radiocomponentTypeIndex = _availableRadiocomponentTypes
                .IndexOf(radiocomponent.Type);

            if (radiocomponentTypeIndex < 0)
            {
                SelectedRadiocomponentTypeIndex = null;
            }
            else
            {
                SelectedRadiocomponentTypeIndex = radiocomponentTypeIndex;
            }

            RadiocomponentValueAsString = radiocomponent.Value.ToString(
                CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Добавляет новый радиокомпонент в коллекцию.
        /// </summary>
        private void AddRadiocomponent()
        {
            if (SelectedRadiocomponentTypeIndex == null
                || _radiocomponents == null)
            {
                return;
            }

            var radiocomponentType = _availableRadiocomponentTypes[
                (int)SelectedRadiocomponentTypeIndex];
            var radiocomponent = RadiocomponentFactory.CreateRadiocomponent(
                radiocomponentType, _radiocomponentValue);
            var printableRadiocomponent
                = new RadiocomponentToPrintableRadiocomponentAdapter(
                    radiocomponent);
            _radiocomponents.Add(printableRadiocomponent);
        }

        #endregion

        #region -- Constructors --

        /// <summary>
        /// Создает экземпляр модели представления
        /// <see cref="AddRadiocomponentViewModel"/>.
        /// </summary>
        /// <param name="availableRadiocomponentTypes">Типы радиокомпонентов,
        /// которые можно будет создавать.</param>
        /// <param name="radiocomponents">Коллекция радиокомпонентов, в
        /// которую будут добавляться новые радиокомпоненты.</param>
        public AddRadiocomponentViewModel(
            List<RadiocomponentType> availableRadiocomponentTypes,
            ObservableCollection<RadiocomponentToPrintableRadiocomponentAdapter> radiocomponents)
        {
            _availableRadiocomponentTypes = availableRadiocomponentTypes;
            _radiocomponents = radiocomponents;

            ValidateAndUpdateRadiocomponentValue();
        }

        #endregion

        #region -- Public properties --

        /// <summary>
        /// Позволяет получить ассоциативный массив, ставящий в соответствие
        /// строковому представлению типа радиокомпонента строковые
        /// представления его физической величины и единицы измерения.
        /// </summary>
        public List<(string, string)>
            RadiocomponentTypeAsStringToQuantityUnitAsStringMap
                => RadiocomponentTypesToTypeAsStringToQuantityUnitAsStringMapConverter
                    .GetRadiocomponentTypeAsStringToQuantityUnitAsStringMap(
                        _availableRadiocomponentTypes);

        /// <summary>
        /// Позволяет задать или получить строковое представление значения
        /// физической величины радиокомпонента.
        /// </summary>
        public string RadiocomponentValueAsString
        {
            get => _radiocomponentValueAsString;
            set
            {
                _radiocomponentValueAsString = value;
                ValidateAndUpdateRadiocomponentValue();
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Позволяет получить значение видимости кнопки создания случайного
        /// радиокомпонента.
        /// </summary>
#if IS_RANDOM_BUTTON_VISIBLE
        public bool IsRandomRadiocomponentButtonVisible { get; } = true;
#else
        public bool IsRandomRadiocomponentButtonVisible { get; } = false;
#endif

        /// <summary>
        /// Позволяет получить или задать индекс выделенного типа
        /// радиокомпонента.
        /// </summary>
        public int? SelectedRadiocomponentTypeIndex
        {
            get => _selectedRadiocomponentTypeIndex;
            set
            {
                _selectedRadiocomponentTypeIndex = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region -- Commands --

        public RelayCommand AddRadiocomponentCommand
            => _adddRadiocomponentCommand ?? (_adddRadiocomponentCommand
                = new RelayCommand(obj => AddRadiocomponent(),
                    obj => _isRadiocomponentValueValid
                           && SelectedRadiocomponentTypeIndex != null));

        public RelayCommand SetRandomRadiocomponentPropertiesCommand
            => _setRandomRadiocomponentPropertiesCommand
               ?? (_setRandomRadiocomponentPropertiesCommand
                   = new RelayCommand(
                       obj => SetRandomRadiocomponentProperties()));

        #endregion
    }
}
