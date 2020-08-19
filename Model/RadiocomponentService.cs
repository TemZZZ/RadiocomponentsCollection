﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Сервисный класс со вспомогательными функциями для работы с
    /// радиокомпонентами
    /// </summary>
    public static class RadiocomponentService
    {
        /// <summary>
        /// Словарь, ставящий в соответствие типам радиокомпонентов их
        /// физические величины и единицы измерения
        /// </summary>
        private static readonly
            Dictionary<RadioComponentType,
                (RadiocomponentQuantity Quantity, RadiocomponentUnit Unit)>
            _radiocomponentTypeToPropertiesMap
                = new Dictionary<RadioComponentType,
                    (RadiocomponentQuantity, RadiocomponentUnit)>
                {
                    [RadioComponentType.Resistor]
                        = (RadiocomponentQuantity.Resistance,
                            RadiocomponentUnit.Ohm),
                    [RadioComponentType.Inductor]
                        = (RadiocomponentQuantity.Inductance,
                            RadiocomponentUnit.Henry),
                    [RadioComponentType.Capacitor]
                        = (RadiocomponentQuantity.Capacitance,
                            RadiocomponentUnit.Farad)
                };

        /// <summary>
        /// Словарь, ставящий в соответствие типам радиокомпонентов их
        /// строковые представления
        /// </summary>
        private static readonly Dictionary<RadioComponentType, string>
            _radiocomponentTypeToStringMap
                = new Dictionary<RadioComponentType, string>
                {
                    [RadioComponentType.Resistor] = "Резистор",
                    [RadioComponentType.Inductor] = "Катушка индуктивности",
                    [RadioComponentType.Capacitor] = "Конденсатор"
                };

        /// <summary>
        /// Словарь, ставящий в соответствие физическим величинам
        /// радиокомпонентов их строковые представления
        /// </summary>
        private static readonly Dictionary<RadiocomponentQuantity, string>
            _radiocomponentQuantityToStringMap
                = new Dictionary<RadiocomponentQuantity, string>
                {
                    [RadiocomponentQuantity.Resistance] = "Сопротивление",
                    [RadiocomponentQuantity.Inductance] = "Индуктивность",
                    [RadiocomponentQuantity.Capacitance] = "Емкость"
                };

        /// <summary>
        /// Словарь, ставящий в соответствие единицам измерений
        /// радиокомпонентов их строковые представления
        /// </summary>
        private static readonly Dictionary<RadiocomponentUnit, string>
            _radiocomponentUnitToStringMap
                = new Dictionary<RadiocomponentUnit, string>
                {
                    [RadiocomponentUnit.Ohm] = "Ом",
                    [RadiocomponentUnit.Henry] = "Гн",
                    [RadiocomponentUnit.Farad] = "Ф"
                };

        /// <summary>
        /// Возвращает соответствующую типу радиокомпонента физическую
        /// величину
        /// </summary>
        /// <param name="type">Тип радиокомпонента</param>
        /// <returns>Физическая величина радиокомпонента</returns>
        public static RadiocomponentQuantity GetRadiocomponentQuantity(
            RadioComponentType type)
        {
            return _radiocomponentTypeToPropertiesMap[type].Quantity;
        }

        /// <summary>
        /// Возвращает соответствующую типу радиокомпонента единицу измерения
        /// </summary>
        /// <param name="type">Тип радиокомпонента</param>
        /// <returns>Единица измерения физической величины
        /// радиокомпонента</returns>
        public static RadiocomponentUnit GetRadiocomponentUnit(
            RadioComponentType type)
        {
            return _radiocomponentTypeToPropertiesMap[type].Unit;
        }
    }
}
