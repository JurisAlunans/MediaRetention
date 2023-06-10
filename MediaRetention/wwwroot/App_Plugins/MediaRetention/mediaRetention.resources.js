(() => {
    function mediaRetentionService($http, umbRequestHelper) {

        let apiRoot = umbRequestHelper.getApiUrl("MediaRetentionBaseUrl", "");

        let request = (method, url, data) =>
            umbRequestHelper.resourcePromise(
                method === 'DELETE' ? $http.delete(url)
                    : method === 'POST' ? $http.post(url, data)
                        : method === 'PUT' ? $http.put(url, data)
                            : $http.get(url),
                'Something broke'
            );

        const service = {
            getAll: (mediaId) => request('GET', apiRoot + 'GetAll?mediaId=' + mediaId),
            delete: (id) => request('DELETE', apiRoot + 'Delete?id=' + id),
            restore: (id) => request('POST', apiRoot + 'Restore?id=' + id),
            download: (id) => umbRequestHelper.downloadFile(apiRoot + "Download?id=" + id)
        };

        return service;
    }

    angular.module("umbraco.services")
        .factory("mediaRetentionService", mediaRetentionService);
})();