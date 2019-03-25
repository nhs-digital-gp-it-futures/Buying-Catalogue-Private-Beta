/**
 * Created by nataki on 3/2/16.
 */
'use strict';

function UUIDGenerator(){
    this.mask = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx';
}

UUIDGenerator.prototype.generate = function(){
    return this.mask.replace(/[xy]/g, function(c) {
        var r = Math.random()*16|0, v = c == 'x' ? r : (r&0x3|0x8);
        return v.toString(16);
    });
};

module.exports = new UUIDGenerator();