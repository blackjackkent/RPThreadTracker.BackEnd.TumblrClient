# RPThreadTracker.BackEnd.TumblrClient
> Tumblr thread status microservice for RPThreadTracker
>
> If you are looking for the back-end application web service API, please see [http://www.github.com/blackjackkent/RPThreadTrackerV3.BackEnd](http://www.github.com/blackjackkent/RPThreadTrackerV3.BackEnd).
>
> If you are looking for the front-end Javascript application code, please see [http://www.github.com/blackjackkent/RPThreadTrackerV3.FrontEnd](http://www.github.com/blackjackkent/RPThreadTrackerV3.FrontEnd).

[![Build status](https://ci.appveyor.com/api/projects/status/t7nenj8hldlywjjt?svg=true)](https://ci.appveyor.com/project/blackjackkent/rpthreadtracker-backend-tumblrclient)
[![codecov](https://codecov.io/gh/blackjackkent/RPThreadTracker.BackEnd.TumblrClient/branch/production/graph/badge.svg)](https://codecov.io/gh/blackjackkent/RPThreadTracker.BackEnd.TumblrClient)


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

## Meta

Rosalind Wills – [@blackjackkent](https://twitter.com/blackjackkent) – rosalind@blackjack-software.com

[https://github.com/blackjackkent/RPThreadTracker.BackEnd.TumblrClient](https://github.com/blackjackkent/RPThreadTracker.BackEnd.TumblrClient/)

## Contributing

1. Fork it (<https://github.com/blackjackkent/RPThreadTracker.BackEnd.TumblrClient/fork>)
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Commit your changes (`git commit -am 'Add some fooBar'`)
4. Push to the branch (`git push origin feature/fooBar`)
5. Create a new Pull Request
