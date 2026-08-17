# plateup-template-builder

plateup-template-builder generates PlateUp! mod templates in an instant, straight from the browser.

### Usage

Open [the template builder](https://starfluxmods.github.io/plateup-template-builder/), pick a project type, fill in your mod's details and hit Generate to download the template as a zip.

### Development

The site is a static page served from the repository root:

- `index.html` - the page itself, including the generation logic.
- `global-references.js` - shared values used by the page, such as the default nuget version.
- `templates/` - the template files the page fetches and packs into the generated zip.
- `kitchenlibassets.js`, `kitchenlibexample.js`, `kitchenlibexamplecustoms.js` - auto-generated file listings for the Unity projects and example customs.

After adding, removing or renaming files under `templates/`, regenerate the file listings with:

```
npm run build-gh
```
