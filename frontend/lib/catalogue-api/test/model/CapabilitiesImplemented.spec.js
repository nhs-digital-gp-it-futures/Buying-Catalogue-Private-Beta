/**
 * catalogue-api
 * NHS Digital GP IT Futures Buying Catalog API
 *
 * OpenAPI spec version: 1.0.0-private-beta
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 *
 * Swagger Codegen version: 2.4.0-SNAPSHOT
 *
 * Do not edit the class manually.
 *
 */

(function(root, factory) {
  if (typeof define === 'function' && define.amd) {
    // AMD.
    define(['expect.js', '../../src/index'], factory);
  } else if (typeof module === 'object' && module.exports) {
    // CommonJS-like environments that support module.exports, like Node.
    factory(require('expect.js'), require('../../src/index'));
  } else {
    // Browser globals (root is window)
    factory(root.expect, root.CatalogueApi);
  }
}(this, function(expect, CatalogueApi) {
  'use strict';

  var instance;

  beforeEach(function() {
    instance = new CatalogueApi.CapabilitiesImplemented();
  });

  var getProperty = function(object, getter, property) {
    // Use getter method if present; otherwise, get the property directly.
    if (typeof object[getter] === 'function')
      return object[getter]();
    else
      return object[property];
  }

  var setProperty = function(object, setter, property, value) {
    // Use setter method if present; otherwise, set the property directly.
    if (typeof object[setter] === 'function')
      object[setter](value);
    else
      object[property] = value;
  }

  describe('CapabilitiesImplemented', function() {
    it('should create an instance of CapabilitiesImplemented', function() {
      // uncomment below and update the code to test CapabilitiesImplemented
      //var instance = new CatalogueApi.CapabilitiesImplemented();
      //expect(instance).to.be.a(CatalogueApi.CapabilitiesImplemented);
    });

    it('should have the property capabilityId (base name: "capabilityId")', function() {
      // uncomment below and update the code to test the property capabilityId
      //var instance = new CatalogueApi.CapabilitiesImplemented();
      //expect(instance).to.be();
    });

    it('should have the property status (base name: "status")', function() {
      // uncomment below and update the code to test the property status
      //var instance = new CatalogueApi.CapabilitiesImplemented();
      //expect(instance).to.be();
    });

    it('should have the property qualityId (base name: "qualityId")', function() {
      // uncomment below and update the code to test the property qualityId
      //var instance = new CatalogueApi.CapabilitiesImplemented();
      //expect(instance).to.be();
    });

    it('should have the property id (base name: "id")', function() {
      // uncomment below and update the code to test the property id
      //var instance = new CatalogueApi.CapabilitiesImplemented();
      //expect(instance).to.be();
    });

    it('should have the property solutionId (base name: "solutionId")', function() {
      // uncomment below and update the code to test the property solutionId
      //var instance = new CatalogueApi.CapabilitiesImplemented();
      //expect(instance).to.be();
    });

    it('should have the property ownerId (base name: "ownerId")', function() {
      // uncomment below and update the code to test the property ownerId
      //var instance = new CatalogueApi.CapabilitiesImplemented();
      //expect(instance).to.be();
    });

  });

}));
