/**
 * Created by nataki on 3/2/16.
 */
'use strict';
var generator = require('../lib/index.js');

for (let i=0; i<10; i++){
    console.log(`UUID #${i} - ${generator.generate()}`);
}
