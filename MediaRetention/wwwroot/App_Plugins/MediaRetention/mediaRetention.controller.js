(() => {

    angular
        .module('umbraco')
        .controller(
            'MediaRetentionController',
            function ($scope, mediaRetentionService, eventsService) {
                var vm = this;

                eventsService.on('app.tabChange', function (event) {
                    
                });           

                function restore() {

                }

                console.log("test");
            }
        );

})();