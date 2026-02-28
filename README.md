# Zero Percent Builder
Zero Percent Builder is a lightweight and simple way to design build pipelines in Unity. It's not finished yet. Currently, you can design pipelines that deploy different build configurations to your local disk, or upload to Steamworks.

# Installation
The easiest way to install ZPI is to install it as a package.
1. Window -> Package Manager
2. Click the "+" button.
3. Select "Add package from git URL."
4. Enter the following URL: `https://github.com/tezemi/ZeroPercentBuilder.git`
5. Click "Add."

# Usage
## Creating a Build Config
You can create a new build config using Build > BuildConfig. A build config defines settings for how to build the game.

## Creating a Pipeline
You can create a new pipeline using Build > Pipeline. A pipeline is a series of steps to be executed one after another when the pipeline is ran. Currently, you can create a new build, get a build from the disk, save a build to the disk, and upload to Steam.

## Options
There is a new category under your user preferences.
- Steam CMD: File for SteamCMD on your local machine. This is needed to upload to Steamworks. You must cache your login before uploading.
- Log Directory: Optional directory to place a log after running a pipeline.
- Steam Username: Needed to upload to Steam.




