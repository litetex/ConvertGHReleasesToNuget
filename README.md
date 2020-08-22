# ConvertGHReleasesToNuget
:warning: This project got completely obsolete, when GitHub introduced [Package Registry](https://github.blog/2019-05-10-introducing-github-package-registry/) and [Actions](https://github.blog/2019-08-08-github-actions-now-supports-ci-cd/). It is now archived :warning:<br/>
:warning: The code is not validated

Syncs the releases of GitHub projects with a folder at your disk, so that you can use it easily as a nuget source

## How can I use it?
### Create a config file
Download the executable and run it with the following parameter:
```SHELL
ConvertGHReleasesToNuget.exe --genconf config.json
```
Now the config file should look like this:
```JS
{
  "AuthConfig": {
    "GitHubPersonalAccessToken": "" // Add your token here (NOTE: the token requires the "repo" scope for private repos)
  },
  "ProjectDownloadConfigs": [
    {
      "Owner": "owner", // Name of the owner where you want to download the resources
      "Repo": "repo", // Name of the repo where you want to download the resources
      "IncludePreRelease": false,
      "DownloadDir": "Download" //The directory where the releases are locally stored
    }
  ]
}
```
Configure it for your purposes.

### Create a source folder
Create a easy accessible folder locally e.g. `C:\LocalNuGetPackages\Sources` and add your configured `config.json` as well as the executable to ``C:\LocalNuGetPackages``

### Run it
Run the executable; it should automatically use the `config.json`. 

If the filename/path of the config is different run  `ConvertGHReleasesToNuget.exe -c <PathToYourConfigFile>`

## Context: Archived and now OSS
This is an old project which was created some time ago. <br/>
It was migrated to NET Core (should work now properly) and is now archived as OSS, so everyone can learn from it :book:
