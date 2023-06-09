(() => {

    angular
        .module('umbraco')
        .controller(
            'MediaRetentionController',
            function ($scope, mediaRetentionService, eventsService, editorState, notificationsService) {
                var vm = this;

                vm.items = [];

                vm.restore = restore;
                vm.deleteFile = deleteFile;
                vm.formatDate = formatDate;

                eventsService.on('app.tabChange', function (event, args) {
                    if (args && args.alias === 'mediaRetention') {
                        loadTableItems();
                    }
                });           

                function restore() {
                    mediaRetentionService.restore(id).then(
                        function (response) {
                            if (response) {
                                notificationsService.success("Success", "Media file has been restored");
                                $route.reload();
                            } else {
                                showErrorNotification();
                            }
                        },
                        function (error) {
                            console.log(error);
                            showErrorNotification();
                        }
                    );
                }

                function deleteFile() {
                    mediaRetentionService.delete(id).then(
                        function (response) {
                            if (response) {
                                notificationsService.success("Success", "Backup file has been deleted");
                                loadTableItems();
                            } else {
                                showErrorNotification();
                            }
                        },
                        function (error) {
                            showErrorNotification();
                        }
                    );
                }

                function loadTableItems() {
                    mediaRetentionService.getAll(editorState.current.id).then(
                        function (response) {
                            vm.items = response;
                        },
                        function (error) {
                            showErrorNotification();
                        }
                    );
                }

                function formatDate(date) {
                    return new Date(date).toLocaleDateString();
                }

                function showErrorNotification() {
                    notificationsService.error("Error", "Operation failed. Please try again.")
                }
            }
        );

})();