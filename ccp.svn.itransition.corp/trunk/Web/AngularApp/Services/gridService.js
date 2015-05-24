

CCPApp.factory('gridService', function () {
    var fac = {};
    fac.getDefaultsPagingOptions = function() {
        fac.pagingOptions.pageSize = 10;
        fac.pagingOptions.currentPage = 1;
    };

    fac.getDefaultsSortingOptions = function () {
        fac.sortInfo.fields = [];
        fac.sortInfo.directions = [];
        fac.sortInfo.columns = [];
    };

    fac.pagingOptions = {
        pageSizes: [10, 20, 50],
        pageSize: 10,
        currentPage: 1
    };

    fac.sortInfo = {
        fields: [],
        directions: [],
        columns:[]
    };

    fac.getDataSource = function (pageNumber, pageSize, sortInfo , filteringOptions) {
        var sortItems = [];
        sortInfo.fields.forEach(function (item, i) {
            sortItems.push({ FieldName: item, Direction: sortInfo.directions[i] });
        });
        return {
                pageNumber: pageNumber,
                pageSize: pageSize,
                sortItems: sortItems,
                filterItems: filteringOptions
            };
    };

    fac.gridOptions = {
        data: 'data',
        columnDefs: [],
        enablePaging: true,
        footerRowHeight: 42,
        footerTemplate: '<div ng-show="showFooter" class="ngFooterPanel" ng-class="{\'ui-widget-content\': jqueryUITheme, \'ui-corner-bottom\': jqueryUITheme}" ng-style="footerStyle()">' +
            '<div class="ngTotalSelectContainer" >' +
            '<div class="ngFooterTotalItems" ng-class="{\'ngNoMultiSelect\': !multiSelect}" >' +
            '<strong style="color:#777777"><span class="ngLabel">{{i18n.ngTotalItemsLabel}} {{maxRows()}}</span><span ng-show="filterText.length > 0" class="ngLabel">({{i18n.ngShowingItemsLabel}} {{totalFilteredItemsLength()}})</span></strong>' +
            '</div>' +
            '<div class="ngFooterSelectedItems" ng-show="multiSelect">' +
            '<span class="ngLabel">{{i18n.ngSelectedItemsLabel}} {{selectedItems.length}}</span>' +
            '</div>' +
            '</div>' +
            '<div class="ngPagerContainer" style="float: right; margin-top: 6px;" ng-show="enablePaging" ng-class="{\'ngNoMultiSelect\': !multiSelect}">' +

            //'<div style="float:left; margin-right: 10px;" class="ngRowCountPicker">' +
            //'<span style="float: left; margin-top: 3px;" class="ngLabel">{{i18n.ngPageSizeLabel}}</span>' +
            //'<select style="float: left;height: 27px; width: 100px" ng-model="pagingOptions.pageSize" >' +
            //'<option ng-repeat="size in pagingOptions.pageSizes">{{size}}</option>' +
            //'</select>' +
            //'</div>' +

            '<div style="float:left; margin-right: 10px; line-height:20px;" class="ngPagerControl footer-btns" style="float: left; min-width: 135px;">' +
            '<div class="btn-group">' +
            '<button class="btn btn-default btn-xs" ng-click="pageToFirst()" ng-disabled="cantPageBackward()" title="{{i18n.ngPagerFirstTitle}}">' +
            '<div class="glyphicon glyphicon-step-backward">' +
            '</div>' +
            '</button>' +
            '<button class="btn btn-default btn-xs" ng-click="pageBackward()" ng-disabled="cantPageBackward()" title="{{i18n.ngPagerPrevTitle}}">' +
            '<div class="glyphicon glyphicon-chevron-left">' +
            '</div>' +
            '</button>' +
            '<span class="pager pull-left">'+
            '<span class="ngPagerCurrent" style="width:30px; height: 33px; margin: 0 4px;">{{pagingOptions.currentPage}}</span>' +
            '<span class=\"ngGridMaxPagesNumber\" ng-show=\"maxPages() > 0\" style="width:30px; height: 33px; vertical-align:initial;  margin-right: 4px">/ {{maxPages()}}</span>' +
            '</span>' +
            '<button class="btn btn-default btn-xs" ng-click="pageForward()" ng-disabled="cantPageForward()" title="{{i18n.ngPagerNextTitle}}">' +
            '<div class="glyphicon glyphicon-chevron-right">' +
            '</div>' +
            '</button>' +
            '<button class="btn btn-default btn-xs" ng-click="pageToLast()" ng-disabled="cantPageToLast()" title="{{i18n.ngPagerLastTitle}}">' +
            '<div class="glyphicon glyphicon-step-forward">' +
            '</div>' +
            '</button>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>',
        multiSelect: false,
        showFooter: true,
        pagingOptions: fac.pagingOptions,
        useExternalSorting: true,
        sortInfo: fac.sortInfo,
        totalServerItems: 'totalServerItems',
        headerRowHeight: 30,
        headerRowTemplate:
            
            '<div>' +
            '<div ng-style=\"{ height: col.headerRowHeight }\" ng-repeat=\"col in renderedColumns\" ng-class=\"col.colIndex()\" class=\"ngHeaderCell\">' +
            '<div class=\"ngVerticalBar\" ng-style=\"{height: col.headerRowHeight}\" ng-class=\"{ ngVerticalBarVisible: !$last }\">&nbsp;</div>'+
            '<div ng-header-cell></div>\r' +
            '</div>' +
            '</div>',
        rowHeight: 25,
        rowTemplate:
            '<div  ng-dblclick="open(row)" ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell {{col.cellClass}}">' +
                '<div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }">&nbsp;' +
                '</div>' +
                '<div ng-cell>' +
                '</div>' +
            '</div>',
    };
    return fac;
});
