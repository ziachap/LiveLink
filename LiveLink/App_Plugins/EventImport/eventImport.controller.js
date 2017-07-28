function EventImportController($scope, notificationsService) {
	var scope = $scope;

	function init() {
		console.log("init");
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

	init();
}