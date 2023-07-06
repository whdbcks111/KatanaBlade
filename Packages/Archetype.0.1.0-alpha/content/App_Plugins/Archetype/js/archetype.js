angular.module("umbraco").controller("Imulus.ArchetypeController", function ($scope, $http, $interpolate, assetsService, angularHelper, notificationsService) {
 
    //$scope.model.value = "";
    //set default value of the model
    //this works by checking to see if there is a model; then cascades to the default model then to an empty fieldset

    var form = angularHelper.getCurrentForm($scope);

    $scope.model.config = $scope.model.config.archetypeConfig;
   
    $scope.model.value = $scope.model.value || { fieldsets: [getEmptyRenderFieldset($scope.model.config.fieldsets[0])] };

    //ini
    $scope.archetypeRenderModel = {};
    initArchetypeRenderModel();
     
    //helper to get $eval the labelTemplate
    $scope.getFieldsetTitle = function(fieldsetConfigModel, fieldsetIndex) {
        var fieldset = $scope.archetypeRenderModel.fieldsets[fieldsetIndex];
        var template = fieldsetConfigModel.labelTemplate;
        var rgx = /{{(.*?)}}*/g;
        var results;
        var parsedTemplate = template;

        while ((results = rgx.exec(template)) !== null) {
            var propertyAlias = results[1];
            var propertyValue = $scope.getPropertyValueByAlias(fieldset, propertyAlias);
            parsedTemplate = parsedTemplate.replace(results[0], propertyValue);
        }

        return parsedTemplate;
    };

    /* add/remove/sort */

    //defines the options for the jquery sortable 
    //i used an ng-model="archetypeRenderModel" so the sort updates the right model
    $scope.sortableOptions = {
        axis: 'y',
        cursor: "move",
        handle: ".handle",
        update: function (ev, ui) {

        },
        stop: function (ev, ui) {

        }
    };

    //handles a fieldset add
    $scope.addRow = function (fieldsetAlias, $index) {
        if ($scope.canAdd())
        {
            if ($scope.model.config.fieldsets)
            {
                var newFieldset = getEmptyRenderFieldset($scope.getConfigFieldsetByAlias(fieldsetAlias));

                if (typeof $index != 'undefined')
                {
                    $scope.archetypeRenderModel.fieldsets.splice($index + 1, 0, newFieldset);
                }
                else
                {
                    $scope.archetypeRenderModel.fieldsets.push(newFieldset);
                }
            }
            newFieldset.collapse = true;
            $scope.focusFieldset(newFieldset);
        }
    }

    //rather than splice the archetypeRenderModel, we're hiding this and cleaning onFormSubmitting
    $scope.removeRow = function ($index) {
        if ($scope.canRemove()) {
            if (confirm('Are you sure you want to remove this?')) {
                $scope.archetypeRenderModel.fieldsets[$index].remove = true;
            }
        }
    }

    //helpers for determining if a user can do something
    $scope.canAdd = function ()
    {
        if ($scope.model.config.maxFieldsets)
        {
            return countVisible() < $scope.model.config.maxFieldsets;
        }

        return true;
    }

    //helper that returns if an item can be removed
    $scope.canRemove = function ()
    {   
        return countVisible() > 1;
    }

    //helper that returns if an item can be sorted
    $scope.canSort = function ()
    {
        return countVisible() > 1;
    }

    //helper, ini the render model from the server (model.value)
    function initArchetypeRenderModel() {
        $scope.archetypeRenderModel = $scope.model.value;

        _.each($scope.archetypeRenderModel.fieldsets, function (fieldset)
        {
            fieldset.remove = false;
            fieldset.collapse = false;
            fieldset.isValid = true;
        });      
    }

    //helper to get the correct fieldset from config
    $scope.getConfigFieldsetByAlias = function(alias) {
        return _.find($scope.model.config.fieldsets, function(fieldset){
            return fieldset.alias == alias;
        });
    }

    //helper to get a property by alias from a fieldset
    $scope.getPropertyValueByAlias = function(fieldset, propertyAlias) {
        var property = _.find(fieldset.properties, function(p) {
            return p.alias == propertyAlias;
        });
        return (typeof property !== 'undefined') ? property.value : '';
    };
    
    //helper for collapsing
    $scope.focusFieldset = function(fieldset){
        
        var iniState;
        
        if(fieldset)
        {
            iniState = fieldset.collapse;
        }
    
        _.each($scope.archetypeRenderModel.fieldsets, function(fieldset){
            if($scope.archetypeRenderModel.fieldsets.length == 1 && fieldset.remove == false)
            {
                fieldset.collapse = false;
                return;
            }
        
            fieldset.collapse = true;
        });
        
        if(iniState)
        {
            fieldset.collapse = !iniState;
        }
    }
    
    //ini
    $scope.focusFieldset();

    //helper returns valid JS or null
    function getValidJson(variable, json)
    {
        if(!json) return null;

        try {
            return eval("(" + json + ")");
        }
        catch (e) {
            console.log("There was an error while using 'eval' on " + variable);
            console.log(json);
            return null;
        }
    }

    //developerMode helpers
    $scope.archetypeRenderModel.toString = stringify;

    //encapsulate stringify (should be built into browsers, not sure of IE support)
    function stringify() {
        return JSON.stringify(this);
    }

    //watch for changes
    $scope.$watch('archetypeRenderModel', function (v) {
        if ($scope.model.config.developerMode) {
            console.log(v);
            if (typeof v === 'string') {
                $scope.archetypeRenderModel = JSON.parse(v);
                $scope.archetypeRenderModel.toString = stringify;
            }
        }
    });

    //helper to count what is visible
    function countVisible()
    {
        var count = 0;

        _.each($scope.archetypeRenderModel.fieldsets, function(fieldset){
            if (fieldset.remove == false) {
                count++;
            }
        });

        return count;
    }

    //helper to sync the model to the renderModel
    function syncModelToRenderModel()
    {
        $scope.model.value = { fieldsets: [] };

        _.each($scope.archetypeRenderModel.fieldsets, function(fieldset){
            if (typeof fieldset != 'function' && !fieldset.remove){

                //clone and clean
                var tempFieldset = JSON.parse(JSON.stringify(fieldset));
                delete tempFieldset.remove;
                delete tempFieldset.isValid;
                delete tempFieldset.collapse;

                _.each(tempFieldset.properties, function(property){
                    delete property.isValid;
                });

                $scope.model.value.fieldsets.push(tempFieldset);
            }
        });
    }

    //helper to add an empty fieldset
    function getEmptyRenderFieldset (fieldsetModel)
    {
        return eval("({ alias: '" + fieldsetModel.alias + "', remove: false, properties: []})");
    }

    //helper for validation
    function getValidation()
    {
        var validation = {}
        validation.isValid = true;
        validation.requiredAliases = [];
        validation.invalidProperties = [];

        //determine which fields are required
        _.each($scope.model.config.fieldsets, function(fieldset){
            _.each(fieldset.properties, function(property){
                if(property.required)
                {
                    validation.requiredAliases.push(property.alias);
                }
            });
        });

        //if nothing required; let's go
        if(validation.requiredAliases.length == 0)
        {
            return validation;
        }

        //otherwise we need to check the required aliases
        _.each($scope.archetypeRenderModel.fieldsets, function(fieldset){
            fieldset.isValid = true;

            _.each(fieldset.properties, function(property){
                property.isValid = true;

                //if a required field
                if(_.find(validation.requiredAliases, function(alias){ return alias == property.alias }))
                {                
                    //TODO: do a better validation test
                    if(property.value == ""){
                        fieldset.isValid = false;
                        property.isValid = false;
                        validation.isValid = false;

                        validation.invalidProperties.push(property);
                    }
                }
            });
        });

        if($scope.model.config.developerMode == '1')
        {
            console.log(validation);
        }

        return validation;
    }

    $scope.getPropertyValidity = function(fieldsetIndex, alias)
    {
        if($scope.archetypeRenderModel.fieldsets[fieldsetIndex])
        {
            var property = _.find($scope.archetypeRenderModel.fieldsets[fieldsetIndex].properties, function(property){
                return property.alias == alias;
            });
        }

        return (typeof property == 'undefined') ? true : property.isValid;
    }

    //sync things up on save
    $scope.$on("formSubmitting", function (ev, args) {
        
        var validation = getValidation();

        if(!validation.isValid)
        {
            notificationsService.warning("Cannot Save Document", "The document could not be saved because of missing required fields.")
            form.$setValidity("archetypeError", false);
        }
        else 
        {
            syncModelToRenderModel();
            form.$setValidity("archetypeError", true);
        }
    });

    //custom js
    if ($scope.model.config.customJsPath) {
        assetsService.loadJs($scope.model.config.customJsPath);
    } 

    //archetype css
    assetsService.loadCss("/App_Plugins/Archetype/css/archetype.css");

    //custom css
    if($scope.model.config.customCssPath)
    {
        assetsService.loadCss($scope.model.config.customCssPath);
    }
});

angular.module("umbraco").controller("Imulus.ArchetypeConfigController", function ($scope, $http, assetsService, propertyEditorService) {
    
    //$scope.model.value = "";
    //console.log($scope.model.value); 

    var newPropertyModel = '{alias: "", remove: false, collapse: false, label: "", helpText: "", view: "", value: "", config: ""}';
    var newFieldsetModel = '{alias: "", remove: false, collapse: false, labelTemplate: "", tooltip: "", icon: "", label: "", headerText: "", footerText: "", properties:[' + newPropertyModel + ']}';
    var defaultFieldsetConfigModel = eval("({showAdvancedOptions: false, hideFieldsetToolbar: false, enableMultipleFieldsets: false, hideFieldsetControls: false, hideFieldsetLabels: false, hidePropertyLabel: false, maxFieldsets: null, fieldsets: [" + newFieldsetModel + "]})");
    
    $scope.model.value = $scope.model.value || defaultFieldsetConfigModel;
    
    initConfigRenderModel();

    //get the available views
    propertyEditorService.getViews().then(function(data){
        $scope.availableViews = data;
    });
      
    $scope.sortableOptions = {
        axis: 'y',
        cursor: "move",
        handle: ".handle",
        update: function (ev, ui) {

        },
        stop: function (ev, ui) {

        }
    };
    
    $scope.focusFieldset = function(fieldset){
        var iniState;
        
        if(fieldset)
        {
            iniState = fieldset.collapse;
        }
        
        _.each($scope.archetypeConfigRenderModel.fieldsets, function(fieldset){
            if($scope.archetypeConfigRenderModel.fieldsets.length == 1 && fieldset.remove == false)
            {
                fieldset.collapse = false;
                return;
            }

            if(fieldset.label)
            {
                fieldset.collapse = true;
            }
            else
            {
                fieldset.collapse = false;
            }
        });
        
        if(iniState)
        {
            fieldset.collapse = !iniState;
        }
    }
    //ini
    $scope.focusFieldset();

    $scope.focusProperty = function(properties, property){
        var iniState;
        
        if(property)
        {
            iniState = property.collapse;
        }

        _.each(properties, function(property){
            if(property.label)
            {
                property.collapse = true;
            }
            else
            {
                property.collapse = false;
            }
        });
        
        if(iniState)
        {
            property.collapse = !iniState;
        }
    }
    
    //setup JSON.stringify helpers
    $scope.archetypeConfigRenderModel.toString = stringify;
    
    //encapsulate stringify (should be built into browsers, not sure of IE support)
    function stringify() {
        return JSON.stringify(this);
    }
    
    //watch for changes
    $scope.$watch('archetypeConfigRenderModel', function (v) {
        //console.log(v);
        if (typeof v === 'string') {     
            $scope.archetypeConfigRenderModel = JSON.parse(v);
            $scope.archetypeConfigRenderModel.toString = stringify;
        }
    }, true);
    
    //helper that returns if an item can be removed
    $scope.canRemoveFieldset = function ()
    {   
        return countVisibleFieldset() > 1;
    }

    //helper that returns if an item can be sorted
    $scope.canSortFieldset = function ()
    {
        return countVisibleFieldset() > 1;
    }
    
    //helper that returns if an item can be removed
    $scope.canRemoveProperty = function (fieldset)
    {   
        return countVisibleProperty(fieldset) > 1;
    }

    //helper that returns if an item can be sorted
    $scope.canSortProperty = function (fieldset)
    {
        return countVisibleProperty(fieldset) > 1;
    }
    
    //helper to count what is visible
    function countVisibleFieldset()
    {
        var count = 0;

        _.each($scope.archetypeConfigRenderModel.fieldsets, function(fieldset){
            if (fieldset.remove == false) {
                count++;
            }
        });

        return count;
    }
    
    function countVisibleProperty(fieldset)
    {
        var count = 0;

        for (var i in fieldset.properties) {
            if (fieldset.properties[i].remove == false) {
                count++;
            }
        }

        return count;
    }
   
    //handles a fieldset add
    $scope.addFieldsetRow = function ($index, $event) {
        $scope.archetypeConfigRenderModel.fieldsets.splice($index + 1, 0, eval("(" + newFieldsetModel + ")"));
        $scope.focusFieldset();
    }
    
    //rather than splice the archetypeConfigRenderModel, we're hiding this and cleaning onFormSubmitting
    $scope.removeFieldsetRow = function ($index) {
        if ($scope.canRemoveFieldset()) {
            if (confirm('Are you sure you want to remove this?')) {
                $scope.archetypeConfigRenderModel.fieldsets[$index].remove = true;
            }
        }
    }
    
    //handles a property add
    $scope.addPropertyRow = function (fieldset, $index) {
        fieldset.properties.splice($index + 1, 0, eval("(" + newPropertyModel + ")"));
    }
    
    //rather than splice the archetypeConfigRenderModel, we're hiding this and cleaning onFormSubmitting
    $scope.removePropertyRow = function (fieldset, $index) {
        if ($scope.canRemoveProperty(fieldset)) {
            if (confirm('Are you sure you want to remove this?')) {
                fieldset.properties[$index].remove = true;
            }
        }
    }
    
    //helper to ini the render model
    function initConfigRenderModel()
    {
        $scope.archetypeConfigRenderModel = $scope.model.value;

        _.each($scope.archetypeConfigRenderModel.fieldsets, function(fieldset){

            fieldset.remove = false;

            if(fieldset.label)
            {
                fieldset.collapse = true;
            }

            _.each(fieldset.properties, function(fieldset){
                fieldset.remove = false;
            });
        });
    }
    
    //sync things up on save
    $scope.$on("formSubmitting", function (ev, args) {
        syncModelToRenderModel();
    });
    
    //helper to sync the model to the renderModel
    function syncModelToRenderModel()
    {
        $scope.model.value = $scope.archetypeConfigRenderModel;
        var fieldsets = [];
        
        _.each($scope.archetypeConfigRenderModel.fieldsets, function(fieldset){
            //check fieldsets
            if (!fieldset.remove) {
                fieldsets.push(fieldset);
                
                var properties = [];

                _.each(fieldset.properties, function(property){
                   if (!property.remove) {
                        properties.push(property);
                    } 
                });

                fieldset.properties = properties;
            }
        });
        
        $scope.model.value.fieldsets = fieldsets;
    }
    
    //archetype css
    assetsService.loadCss("/App_Plugins/Archetype/css/archetype.css");
});

angular.module("umbraco").directive('archetypeProperty', function ($compile, $http) {
    
    function getFieldsetByAlias(fieldsets, alias)
    {
        for (var i in fieldsets)
        {
            if (fieldsets[i].alias == alias)
            {
                return fieldsets[i];
            }
        }
    }

    function getPropertyIdByAlias(properties, alias)
    {
        for (var i in properties)
        {
            if (properties[i].alias == alias) {
                return i;
            }
        }
    }

    var linker = function (scope, element, attrs) {
         
        var configFieldsetModel = getFieldsetByAlias(scope.archetypeConfig.fieldsets, scope.fieldset.alias);

        var view = configFieldsetModel.properties[scope.propertyConfigIndex].view;
        var label = configFieldsetModel.properties[scope.propertyConfigIndex].label;
        var config = configFieldsetModel.properties[scope.propertyConfigIndex].config;
        var alias = configFieldsetModel.properties[scope.propertyConfigIndex].alias;
        var defaultValue = configFieldsetModel.properties[scope.propertyConfigIndex].value;
        
        //try to convert the config to a JS object
        if(config && typeof config == 'string'){
            try{
                if(scope.archetypeConfig.developerMode == '1'){
                    console.log("Trying to eval config: " + config); 
                }
                config = eval("(" + config + ")");
            }
            catch(exception)
            {
                if(scope.archetypeConfig.developerMode == '1'){
                    console.log("Failed to eval config."); 
                }
            }
        }

        if(config && scope.archetypeConfig.developerMode == '1'){
            console.log("Config post-eval: " + config); 
        }

        //try to convert the defaultValue to a JS object
        if(defaultValue && typeof defaultValue == 'string'){
            try{
                if(scope.archetypeConfig.developerMode == '1'){
                    console.log("Trying to eval default value: " + defaultValue); 
                }
                defaultValue = eval("(" + defaultValue + ")");
            }
            catch(exception)
            {
                if(scope.archetypeConfig.developerMode == '1'){
                    console.log("Failed to eval defaultValue."); 
                }
            }
        }

        if(defaultValue && scope.archetypeConfig.developerMode == '1'){
            console.log("Default value post-eval: " + defaultValue); 
        }

        if (view)
        {
            $http.get(view).success(function (data) {
                if (data) {
                    if (scope.archetypeConfig.developerMode == '1')
                    {
                        console.log(scope);
                    }

                    var rawTemplate = data;

                    //define the initial model and config
                    scope.model = {};
                    scope.model.config = {};

                    //ini the property value after test to make sure a prop exists in the renderModel
                    var renderModelPropertyIndex = getPropertyIdByAlias(scope.archetypeRenderModel.fieldsets[scope.fieldsetIndex].properties, alias);

                    if (!renderModelPropertyIndex)
                    {
                        scope.archetypeRenderModel.fieldsets[scope.fieldsetIndex].properties.push(eval("({alias: '" + alias + "', value:'" + defaultValue + "'})"));
                        renderModelPropertyIndex = getPropertyIdByAlias(scope.archetypeRenderModel.fieldsets[scope.fieldsetIndex].properties, alias);
                    }
                    scope.model.value = scope.archetypeRenderModel.fieldsets[scope.fieldsetIndex].properties[renderModelPropertyIndex].value;

                    //set the config from the prevalues
                    scope.model.config = config;

                    //some items need an alias
                    scope.model.alias = "scope-" + scope.$id;

                    //watch for changes since there is no two-way binding with the local model.value
                    scope.$watch('model.value', function (newValue, oldValue) {
                        scope.archetypeRenderModel.fieldsets[scope.fieldsetIndex].properties[renderModelPropertyIndex].value = newValue;
                    });

                    element.html(rawTemplate).show();
                    $compile(element.contents())(scope);
                }
            });
        }
    }

    return {
        restrict: "E",
        rep1ace: true,
        link: linker,
        scope: {
            property: '=',
            propertyConfigIndex: '=',
            archetypeConfig: '=',
            fieldset: '=',
            fieldsetIndex: '=',
            archetypeRenderModel: '='
        }
    }
});
angular.module('umbraco').factory('propertyEditorService', function($q, $http, umbRequestHelper){
    return {
        getViews: function() {
            return umbRequestHelper.resourcePromise(
                $http.get("/App_Plugins/Archetype/js/config.views.js"), 'Failed to retreive config for views.'
            );
        }
    };
});