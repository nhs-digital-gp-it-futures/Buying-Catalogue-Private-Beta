# UUID generator

Simple generation of RFC4122 UUIDS.

## What

Generate UUIDs in the format 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'

## Installation

```bash
npm install node-uuid-generator --save
```

## Usage

```javascript

var generator = require('node-uuid-generator');

generator.generate();

```

## API Reference

Depending on the size of the project, if it is small and simple enough the reference docs can be added to the README. For medium size to larger projects it is important to at least provide a link to where the API reference docs live.

## Tests

```bash
npm install
npm test
```

## License

Released under the [MIT] license. Copyright (c) 2016 nataki.

[MIT]: https://github.com/nataki/node-UUID-generator/blob/master/LICENSE.md
