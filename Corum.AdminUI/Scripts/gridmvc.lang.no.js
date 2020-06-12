function LocalizeMvcGrid() {

    debugger;
    window.GridMvc = window.GridMvc || {};
    window.GridMvc.lang = window.GridMvc.lang || {};
    GridMvc.lang.ru = {
        filterTypeLabel: "Тип фильтра: ",
        filterValueLabel: "Значение:",
        applyFilterButtonText: "Применить",
        filterSelectTypes: {
            Equals: "Равно",
            StartsWith: "Начинается с",
            Contains: "Содержит",
            EndsWith: "Оканчивается на",
            GreaterThan: "Больше чем",
            LessThan: "Меньше чем"
        },
        code: 'ru',
        boolTrueLabel: "Да",
        boolFalseLabel: "Нет",
        clearFilterLabel: "Очистить"
    };
}