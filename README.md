# RPThreadTracker.BackEnd.TumblrClient
> Tumblr thread status microservice for RPThreadTracker
>
> If you are looking for the back-end application web service API, please see [http://www.github.com/blackjackkent/RPThreadTrackerV3.BackEnd](http://www.github.com/blackjackkent/RPThreadTrackerV3.BackEnd).
>
> If you are looking for the front-end Javascript application code, please see [http://www.github.com/blackjackkent/RPThreadTrackerV3.FrontEnd](http://www.github.com/blackjackkent/RPThreadTrackerV3.FrontEnd).

[![Build status](https://ci.appveyor.com/api/projects/status/t7nenj8hldlywjjt?svg=true)](https://ci.appveyor.com/project/blackjackkent/rpthreadtracker-backend-tumblrclient)
[![codecov](https://codecov.io/gh/blackjackkent/RPThreadTracker.BackEnd.TumblrClient/branch/production/graph/badge.svg)](https://codecov.io/gh/blackjackkent/RPThreadTracker.BackEnd.TumblrClient)
[![Discord](https://img.shields.io/discord/375365160057176064?color=7389D8&label=Discord&logo=discord)](https://discord.gg/k4gDad5)


This is a microservice called by [RPThreadTrackerV3.FrontEnd](https://github.com/blackjackkent/RPThreadTrackerV3.FrontEnd) to retrieve the current status of tracked threads on the Tumblr platform. It also provides an endpoint for retrieving recent news posts from the Tracker's news blog.

## Usage example

Request:

```http
POST /api/thread HTTP/1.1
Host: {baseUrl}
Content-Type: application/json

[
{"postId": "62854047895", "characterUrlIdentifier": "cmdr-blackjack-shepard", "partnerUrlIdentifier": ""},
{"postId": "79443357041", "characterUrlIdentifier": "cmdr-blackjack-shepard", "partnerUrlIdentifier": "blackjackkent-test", "dateMarkedQueued": "2017-08-06T20:26:06.503Z"}
]
```

Response:
```json
[
  {
    "PostId": "62854047895",
    "LastPostDate": "2013-10-02T00:11:34Z",
    "LastPosterUrlIdentifier": "cmdr-blackjack-shepard",
    "LastPostUrl": "http://cmdr-blackjack-shepard.tumblr.com/post/62854047895",
    "IsCallingCharactersTurn": false,
    "IsQueued": false
  },
  {
    "PostId": "122728533704",
    "LastPostDate": "2015-12-28T16:21:07Z",
    "LastPosterUrlIdentifier": "blackjackkent-test",
    "LastPostUrl": "http://blackjackkent-test.tumblr.com/post/136117843415",
    "IsCallingCharactersTurn": true,
    "IsQueued": false
  }
]
```

## Running the Application Locally

You will need to have the .NET Core SDK v.2.1.4 or later installed on your local machine to develop this application.

1. Create a fork of this repository to your own GitHub account (<https://github.com/blackjackkent/RPThreadTracker.BackEnd.TumblrClient/fork>).
2. Clone the forked repository to your local machine.
3. Check out a new feature branch in your local copy of the code.
4. Generate a set of Tumblr OAuth credentials.
	* First, register your application [here](https://www.tumblr.com/oauth/apps).
	* Use the consumer key and consumer secret you just generated to generate an OAuth token and OAuth secret associated with your Tumblr account using the form [here](https://api.tumblr.com/console/calls/user/info).
	* Duplicate the file at `./RPThreadTrackerV3.BackEnd.TumblrClient/appsettings.secure.example.json` and name it `appsettings.secure.json`, and update this new file with the consumer key, consumer secret, OAuth token, and OAuth secret that you just generated.

Once running, the application will be available at `http://localhost:58075`.

## Running Unit Tests

The application uses [XUnit](https://xunit.github.io/) and associated libraries for unit testing across all parts of the application. Any changes to the code should be appropriately unit tested to maintain code coverage. Test files should be added to the `RPThreadTrackerV3.BackEnd.TumblrClient.Test` project following existing patterns.

You can run all unit tests using your preferred C# test runner. To generate a code coverage report, run `./coverage.sh` from the project root. The generated coverage information will appear in a folder called `./reports`.

## External Dependencies

This application has no external dependencies besides the Tumblr API. Make sure you have followed the instructions in `Running the Application Locally` regarding setting up your Tumblr OAuth tokens.

## Submitting a Change

1. Commit your changes to your feature branch and push it to your forked repository.
2. Open a pull request to the repository at https://github.com/blackjackkent/RPThreadTracker.BackEnd.TumblrClient.

## Meta

Rosalind Wills – [@blackjackkent](https://twitter.com/blackjackkent) – rosalind@blackjack-software.com

[https://github.com/blackjackkent/RPThreadTracker.BackEnd.TumblrClient](https://github.com/blackjackkent/RPThreadTracker.BackEnd.TumblrClient/)
