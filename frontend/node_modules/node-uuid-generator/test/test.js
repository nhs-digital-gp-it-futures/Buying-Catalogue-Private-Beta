'use strict';

var expect = require('chai').expect;
var generator = require('../lib/index.js');

describe('UUID generator', function(){
    describe('API', function(){
        it('should have "generate" method', function(){
            expect(generator).to.have.property("generate");
            expect(generator.generate).to.be.instanceof(Function);
        });
    });
    describe("Generate", function(){
        it('should generate string', function(){
            var uuid = generator.generate();
            expect(uuid).to.be.a('string');
        });

        it('should generate string of corresponding format', function(){
            var uuid = generator.generate();
            expect(/^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i
                .test(uuid)).to.be.true;
        });

        it('should generate unique string', function(){
            var uuid1 = generator.generate(),
                uuid2 = generator.generate();
            expect(uuid1).not.to.be.equal(uuid2);
        });
    });

});
