
FROM microsoft/dotnet:2.1-runtime

WORKDIR /app

COPY . ./RPThreadTrackerV3/BackEnd/TumblrClient/bin/Release/netcoreapp2.1/publish

ENTRYPOINT dotnet RPThreadTrackerV3.BackEnd.TumblrClient.dll
