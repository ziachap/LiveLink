function VenueEventImportController($scope, notificationsService, editorState) {
	var scope = $scope;
	var id = editorState.current.id;

	function init() {
		console.log("init " + id);
	}

	scope.importFacebookVenueEvents = function () {
		console.log("clicked");
		notificationsService.success('Success', 'Import started..');
		
		$.ajax({
			url: '/api/import-facebook-venue-events/',
			data: 'id=' + id + '&limit=20',
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