require_relative 'scripts/setup'
require_relative 'scripts/copy-dependencies'
require_relative 'scripts/utils'
require_relative 'scripts/coverage'

task :cover do
	filter = []
	filter << "+[MoBi.Core]*"
	filter << "+[MoBi.Assets]*"
	filter << "+[MoBi.Presentation]*"

	Coverage.cover(filter , "MoBi.Tests.csproj")
end

task :create_setup, [:product_version, :configuration, :smart_xls_package, :smart_xls_version] do |t, args|
	update_smart_xls args

	src_dir = src_dir_for(args.configuration)
	relative_src_dir = relative_src_dir_for(args.configuration)


	#Ignore files from automatic harvesting that will be installed specifically
	harvest_ignored_files = [
		'MoBi.exe',
		'Standard Molecule.pkml'
	]

	#Files required for setup creation only
	setup_files	 = [
		"#{relative_src_dir}/ChartLayouts/**/*.{wxs,xml}",
		"#{relative_src_dir}/TeXTemplates/**/*.*",
		'examples/**/*.{wxs,pkml,mbp3}',
		'src/Data/**/*.*',
		'src/MoBi.Assets/Resources/*.ico',
		'documentation/*.pdf',
		'dimensions/*.xml',
		'pkparameters/*.xml',
		'setup/setup.wxs',
		'setup/**/*.{msm,rtf,bmp}',
		'Open Systems Pharmacology Suite License.pdf'
	]

	Rake::Task['setup:create'].execute(OpenStruct.new(
		solution_dir: solution_dir,
		src_dir: src_dir,  
		setup_dir: setup_dir,  
		product_name: product_name, 
		product_version: args.product_version,
		harvest_ignored_files: harvest_ignored_files,		
		suite_name: suite_name,
		setup_files: setup_files,
		manufacturer: manufacturer
		))
end

task :create_portable_setup, [:product_version, :configuration, :package_name] do |t, args|
	src_dir = src_dir_for(args.configuration)
	relative_src_dir = relative_src_dir_for(args.configuration)

	#Files required for setup creation only and that will not be harvested automatically
	setup_files	 = [
		'Open Systems Pharmacology Suite License.pdf',
		'documentation/*.pdf',
		'dimensions/*.xml',
		'pkparameters/*.xml',
		'src/Data/*.xml',
		'setup/**/*.{rtf}',
	]

	setup_folders = [
		'examples/**/*.{pkml,mbp3}',
		"#{relative_src_dir}/ChartLayouts/**/*.{xml}",
		"#{relative_src_dir}/TeXTemplates/**/*.{json,sty,tex}"
	]

	Rake::Task['setup:create_portable'].execute(OpenStruct.new(
		solution_dir: solution_dir,
		src_dir: src_dir, 
		setup_dir: setup_dir,  
		product_name: product_name, 
		product_version: args.product_version,
		suite_name: suite_name,
		setup_files: setup_files,
		setup_folders: setup_folders,
		package_name: args.package_name
		))
end


task :update_go_license, [:file_path, :license] do |t, args|
	Utils.update_go_diagram_license args.file_path, args.license
end	

def relative_src_dir_for(configuration)
	File.join( 'src', 'MoBi', 'bin', configuration, 'net472')
end

def src_dir_for(configuration)
	File.join(solution_dir, relative_src_dir_for(configuration))
end

def update_smart_xls(args) 
	require_relative 'scripts/smartxls'

	if (!args.smart_xls_package || !args.smart_xls_version)
		return
	end

	src_dir = src_dir_for(args.configuration)
	SmartXls.update_smart_xls src_dir, args.smart_xls_package, args.smart_xls_version
end

task :postclean do |t, args| 
	packages_dir =  src_dir_for("Debug")

	all_users_dir = ENV['ALLUSERSPROFILE']
	all_users_application_dir = File.join(all_users_dir, manufacturer, product_name, '9.0')

	copy_dependencies solution_dir,  all_users_application_dir do
		copy_files 'Data', ['xml', 'mbdt']
		copy_file 'src/Data/AllCalculationMethods.pkml'
		copy_dimensions_xml
		copy_pkparameters_xml
	end

	copy_dependencies solution_dir,  File.join(all_users_application_dir, 'Templates') do
		copy_templates_pkml
	end

	copy_dependencies packages_dir,   File.join(all_users_application_dir, 'ChartLayouts') do
		copy_files 'OSPSuite.Presentation', 'xml'
	end

	copy_dependencies packages_dir,   File.join(all_users_application_dir, 'TeXTemplates', 'StandardTemplate') do
		copy_files 'StandardTemplate', '*'
	end
end

private

def solution_dir
	File.dirname(__FILE__)
end

def manufacturer
	'Open Systems Pharmacology'
end

def product_name
	'MoBi'
end

def suite_name
	'Open Systems Pharmacology Suite'
end

def setup_dir
	File.join(solution_dir, 'setup')
end