function EventImportController($scope, notificationsService) {
	var scope = $scope;

	function init() {
	}

	scope.importFacebookEvents = function () {
		notificationsService.success('Success', 'Import started..');
		$.ajax({
			url: '/api/import-facebook-events/',
			data: 'limit=20',
			type: "GET",
			cache: false,
			success: function (response) {
				notificationsService.success('Success', 'Imported successfully! (I think)');
			},
			error: function (e) {
				notificationsService.error('An error occurred', e.statusText.toLowerCase());
			},
			complete: function () {
			}
		});
	}

	scope.cleanupEvents = function () {
		notificationsService.success('Success', 'Cleanup started..');
		$.ajax({
			url: '/api/cleanup-events/',
			type: "GET",
			cache: false,
			success: function (response) {
				var count = response.Length;
				notificationsService.success('Success', 'Deleted some events successfully! ' + count);
			},
			error: function (e) {
				notificationsService.error('An error occurred', e.statusText.toLowerCase());
			},
			complete: function () {
			}
		});
	}

	init();
}