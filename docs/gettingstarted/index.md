# Getting Started

## Contributing to RPThreadTrackerV3.BackEnd.TumblrClient

All contributions to RPThreadTrackerV3.BackEnd.TumblrClient can be made via pull requests to https://github.com/blackjackkent/RPThreadTracker.BackEnd.TumblrClient. To generate a pull request with your code changes:

1. Fork the repository (https://github.com/blackjackkent/RPThreadTracker.BackEnd.TumblrClient/fork)
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Commit your changes (`git commit -am 'Add some fooBar'`)
4. Push to the branch (`git push origin feature/fooBar`)
5. Create a new pull request on GitHub

## Running the code locally

RPThreadTrackerV3.BackEnd.TumblrClient is written in the .NET Core 2.1 framework and uses SQL Server for error logging with NLog. In order to run the project locally on your machine:

1. Fork the repository and clone it to your computer.
2. Install SQL Server if you do not already have it installed and create a database named `RPThreadTracker` with a table named `dbo.Log`.
4. Update `RPThreadTrackerV3.BackEnd/appSettings.json` with the connection string to your database if necessary.
5. Duplicate `RPThreadTrackerV3.BackEnd/appsettings.secure.example.json` and rename the file to `appsettings.secure.json`.
6. Update your new `appsettings.secure.json` file with OAuth authentication information for the Tumblr API. (See [https://www.tumblr.com/docs/en/api/v2](https://www.tumblr.com/docs/en/api/v2) for information on how to generate OAuth credentials for Tumblr.)
7. If you are planning to run the [front-end project](https://github.com/blackjackkent/RPThreadTrackerV3.FrontEnd) as well, be sure that the `CorsUrl` key in `appsettings.json` is set to the URL where that application will be running.

## Questions?

Contact the developer at [rosalind@blackjack-software.com](mailto:rosalind@blackjack-software.com) or [submit a GitHub issue](https://github.com/blackjackkent/RPThreadTracker.BackEnd.TumblrClient/issues).
