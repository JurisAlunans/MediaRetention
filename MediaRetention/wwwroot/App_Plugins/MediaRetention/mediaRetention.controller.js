(() => {
    angular
        .module('umbraco')
        .controller(
            'MediaRetentionController',
            function (mediaRetentionService, eventsService, editorState, notificationsService, $route) {
                var vm = this;

                vm.items = [];

                vm.restoreFile = restoreFile;
                vm.deleteFile = deleteFile;
                vm.downloadFile = downloadFile;
                vm.formatDate = formatDate;

                eventsService.on('app.tabChange', function (event, args) {
                    if (args && args.alias === 'mediaRetention') {
                        loadTableItems();
                    }
                });           

                function restoreFile(id) {
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

                function deleteFile(id) {
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

                function downloadFile(id) {
                    mediaRetentionService.download(id).then(
                        function (response) {
                              notificationsService.success('Success', 'File downloaded');
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
                    return new Date(date).toLocaleString();
                }

                function showErrorNotification() {
                    notificationsService.error("Error", "Operation failed. Please try again.")
                }
            }
        );
})();