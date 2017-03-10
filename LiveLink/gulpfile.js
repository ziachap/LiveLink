/// <binding BeforeBuild='build' />
var gulp = require('gulp'),
	sass = require('gulp-sass'),
	concat = require('gulp-concat'),
	uglify = require('gulp-uglify'),
	rev = require('gulp-rev'),
	revdel = require('gulp-rev-delete-original'),
	filter = require('gulp-filter');

gulp.task('css', function () {
	return gulp.src([
		'!./gulp/scss/amp/**/*.scss',
		'!./gulp/scss/rte.scss',
		'./gulp/scss/**/*.scss'
	])
	.pipe(sass({ outputStyle: 'compressed' }))
	.pipe(gulp.dest('./css'))
	.pipe(filter('**/*.css'))
});

gulp.task('rte', function () {
	return gulp.src('./gulp/scss/rte.scss')
	.pipe(sass({ outputStyle: 'compressed' }))
	.pipe(gulp.dest('./css'));
});

gulp.task('amp', function () {
	return gulp.src('./gulp/scss/amp/**/*.scss')
	.pipe(sass({ outputStyle: 'compressed' }))
	.pipe(gulp.dest('./css/amp/'));
});

gulp.task('js', function () {
	return gulp.src([
		'./gulp/scripts/plugins/**/*.js',
		'./gulp/scripts/mvcfoolproof.unobtrusive.min.js',
		'./gulp/scripts/site.js',
		'./gulp/scripts/modules/**/*.js'
	])
	.pipe(concat('site.js'))
	.pipe(uglify())
	.pipe(gulp.dest('./js'))
	.pipe(filter('**/*.js'));
});

gulp.task('build', ['css', 'rte', 'amp', 'js']);

gulp.task('watch', function () {
	gulp.watch([
		'!./gulp/scss/amp/**/*.scss',
		'!./gulp/scss/rte.scss',
		'./gulp/scss/**/*.scss'
	], ['css']);
	gulp.watch('./gulp/scss/rte.scss', ['rte']);
	gulp.watch('./gulp/scss/amp/**/*.scss', ['amp']);
	gulp.watch('./gulp/scripts/**/*.js', ['js']);
});