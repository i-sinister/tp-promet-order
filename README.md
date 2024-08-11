Test task for an interview

# debugging

SpaProxy is not used so in order to debug project do:

1. start debugger for `pos.api` project in vs/vscode
2. open terminal and start `ng server`

# publishing

Run `dotnet publish ./src/pos.api/pos.api.csproj` to publish both api and ui into `published` folder.

By default page is available as `http://localhost:5000/index.html`.
