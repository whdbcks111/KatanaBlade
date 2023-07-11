param($installPath, $toolsPath, $package, $project)

#Output value for debugging
Write-Host "Install path: " $installPath
Write-Host "Tools path: " $toolsPath
Write-Host "Package: " $package
Write-Host "Project: " $project

$projectDestinationPath = Split-Path $project.FullName -Parent
Write-Host "Project Destination Path: " $projectDestinationPath


if ($project) {
	

	$project.ProjectItems.Item("css").ProjectItems.Item("dummy.txt").Delete()
	$project.ProjectItems.Item("masterpages").ProjectItems.Item("dummy.txt").Delete()
	$project.ProjectItems.Item("media").ProjectItems.Item("dummy.txt").Delete()
	$project.ProjectItems.Item("scripts").ProjectItems.Item("dummy.txt").Delete()
	$project.ProjectItems.Item("usercontrols").ProjectItems.Item("dummy.txt").Delete()

	#Write to console
	Write-Host "All DONE"
}