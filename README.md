# statiq-tutorial
[![Deploy to Netlify](https://www.netlify.com/img/deploy/button.svg)](https://app.netlify.com/start/deploy?repository=https://github.com/makma/statiq-tutorial)

This is a source code for the [Statiq](https://statiq.dev/) [Tutorial article](https://www.netlify.com/blog/2021/01/22/why-should-.net-developers-be-interested-in-jamstack/) published on Netlify.
The output of this project for published content is available on https://statiq-tutorial.netlify.app/.
The output of this project for unpublished content is available on https://statiq-tutorial-preview.netlify.app/.

## Data Source
The project uses data from the `input` directory markdown files. It also uses content from headless CMS [Kontent](https://kontent.ai/) from the `60bc2ac5-f3d6-004c-063e-56febdd011c2` projectId.

## Models
Generated models are stored in the `Models` directory. They are generated by [Kontent Generator](https://github.com/Kentico/kontent-generators-net) tool. `GenerateModels.ps1` is a Powershell script which gets `projectId` from `appsettings.json` and is responsible for generating these models.

## Pipelines
Pipelines are responsible for processing markdown files or getting content from the headless CMS. This content is transformed into HTML documents using rendering modules.

## How to run
Output directory is created by running `dotnet run` or `dotnet run --preview`. ❕.NET5 required
