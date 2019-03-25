'use strict';

exports.__esModule = true;

var _asyncToGenerator2 = require('babel-runtime/helpers/asyncToGenerator');

var _asyncToGenerator3 = _interopRequireDefault(_asyncToGenerator2);

var _lodash = require('lodash');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

class TestFileCompilerBase {
    constructor() {
        const escapedExt = (0, _lodash.escapeRegExp)(this.getSupportedExtension());

        this.supportedExtensionRe = new RegExp(`${escapedExt}$`);
    }

    _hasTests() /* code */{
        throw new Error('Not implemented');
    }

    getSupportedExtension() {
        throw new Error('Not implemented');
    }

    precompile() /* testFilesInfo */{
        return (0, _asyncToGenerator3.default)(function* () {
            throw new Error('Not implemented');
        })();
    }

    compile() /* code, filename */{
        return (0, _asyncToGenerator3.default)(function* () {
            throw new Error('Not implemented');
        })();
    }

    execute() /* compiledCode, filename */{
        return (0, _asyncToGenerator3.default)(function* () {
            throw new Error('Not implemented');
        })();
    }

    canCompile(code, filename) {
        return this.supportedExtensionRe.test(filename);
    }

    get canPrecompile() {
        return false;
    }

    cleanUp() {
        // NOTE: Optional. Do nothing by default.
    }
}
exports.default = TestFileCompilerBase;
module.exports = exports['default'];
//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIi4uLy4uLy4uL3NyYy9jb21waWxlci90ZXN0LWZpbGUvYmFzZS5qcyJdLCJuYW1lcyI6WyJUZXN0RmlsZUNvbXBpbGVyQmFzZSIsImNvbnN0cnVjdG9yIiwiZXNjYXBlZEV4dCIsImdldFN1cHBvcnRlZEV4dGVuc2lvbiIsInN1cHBvcnRlZEV4dGVuc2lvblJlIiwiUmVnRXhwIiwiX2hhc1Rlc3RzIiwiRXJyb3IiLCJwcmVjb21waWxlIiwiY29tcGlsZSIsImV4ZWN1dGUiLCJjYW5Db21waWxlIiwiY29kZSIsImZpbGVuYW1lIiwidGVzdCIsImNhblByZWNvbXBpbGUiLCJjbGVhblVwIl0sIm1hcHBpbmdzIjoiOzs7Ozs7OztBQUFBOzs7O0FBRWUsTUFBTUEsb0JBQU4sQ0FBMkI7QUFDdENDLGtCQUFlO0FBQ1gsY0FBTUMsYUFBYSwwQkFBUyxLQUFLQyxxQkFBTCxFQUFULENBQW5COztBQUVBLGFBQUtDLG9CQUFMLEdBQTRCLElBQUlDLE1BQUosQ0FBWSxHQUFFSCxVQUFXLEdBQXpCLENBQTVCO0FBQ0g7O0FBRURJLGdCQUFXLFVBQVk7QUFDbkIsY0FBTSxJQUFJQyxLQUFKLENBQVUsaUJBQVYsQ0FBTjtBQUNIOztBQUVESiw0QkFBeUI7QUFDckIsY0FBTSxJQUFJSSxLQUFKLENBQVUsaUJBQVYsQ0FBTjtBQUNIOztBQUVLQyxjQUFOLEdBQWtCLG1CQUFxQjtBQUFBO0FBQ25DLGtCQUFNLElBQUlELEtBQUosQ0FBVSxpQkFBVixDQUFOO0FBRG1DO0FBRXRDOztBQUVLRSxXQUFOLEdBQWUsb0JBQXNCO0FBQUE7QUFDakMsa0JBQU0sSUFBSUYsS0FBSixDQUFVLGlCQUFWLENBQU47QUFEaUM7QUFFcEM7O0FBRUtHLFdBQU4sR0FBZSw0QkFBOEI7QUFBQTtBQUN6QyxrQkFBTSxJQUFJSCxLQUFKLENBQVUsaUJBQVYsQ0FBTjtBQUR5QztBQUU1Qzs7QUFFREksZUFBWUMsSUFBWixFQUFrQkMsUUFBbEIsRUFBNEI7QUFDeEIsZUFBTyxLQUFLVCxvQkFBTCxDQUEwQlUsSUFBMUIsQ0FBK0JELFFBQS9CLENBQVA7QUFDSDs7QUFFRCxRQUFJRSxhQUFKLEdBQXFCO0FBQ2pCLGVBQU8sS0FBUDtBQUNIOztBQUVEQyxjQUFXO0FBQ1A7QUFDSDtBQXJDcUM7a0JBQXJCaEIsb0IiLCJmaWxlIjoiY29tcGlsZXIvdGVzdC1maWxlL2Jhc2UuanMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBlc2NhcGVSZWdFeHAgYXMgZXNjYXBlUmUgfSBmcm9tICdsb2Rhc2gnO1xuXG5leHBvcnQgZGVmYXVsdCBjbGFzcyBUZXN0RmlsZUNvbXBpbGVyQmFzZSB7XG4gICAgY29uc3RydWN0b3IgKCkge1xuICAgICAgICBjb25zdCBlc2NhcGVkRXh0ID0gZXNjYXBlUmUodGhpcy5nZXRTdXBwb3J0ZWRFeHRlbnNpb24oKSk7XG5cbiAgICAgICAgdGhpcy5zdXBwb3J0ZWRFeHRlbnNpb25SZSA9IG5ldyBSZWdFeHAoYCR7ZXNjYXBlZEV4dH0kYCk7XG4gICAgfVxuXG4gICAgX2hhc1Rlc3RzICgvKiBjb2RlICovKSB7XG4gICAgICAgIHRocm93IG5ldyBFcnJvcignTm90IGltcGxlbWVudGVkJyk7XG4gICAgfVxuXG4gICAgZ2V0U3VwcG9ydGVkRXh0ZW5zaW9uICgpIHtcbiAgICAgICAgdGhyb3cgbmV3IEVycm9yKCdOb3QgaW1wbGVtZW50ZWQnKTtcbiAgICB9XG5cbiAgICBhc3luYyBwcmVjb21waWxlICgvKiB0ZXN0RmlsZXNJbmZvICovKSB7XG4gICAgICAgIHRocm93IG5ldyBFcnJvcignTm90IGltcGxlbWVudGVkJyk7XG4gICAgfVxuXG4gICAgYXN5bmMgY29tcGlsZSAoLyogY29kZSwgZmlsZW5hbWUgKi8pIHtcbiAgICAgICAgdGhyb3cgbmV3IEVycm9yKCdOb3QgaW1wbGVtZW50ZWQnKTtcbiAgICB9XG5cbiAgICBhc3luYyBleGVjdXRlICgvKiBjb21waWxlZENvZGUsIGZpbGVuYW1lICovKSB7XG4gICAgICAgIHRocm93IG5ldyBFcnJvcignTm90IGltcGxlbWVudGVkJyk7XG4gICAgfVxuXG4gICAgY2FuQ29tcGlsZSAoY29kZSwgZmlsZW5hbWUpIHtcbiAgICAgICAgcmV0dXJuIHRoaXMuc3VwcG9ydGVkRXh0ZW5zaW9uUmUudGVzdChmaWxlbmFtZSk7XG4gICAgfVxuXG4gICAgZ2V0IGNhblByZWNvbXBpbGUgKCkge1xuICAgICAgICByZXR1cm4gZmFsc2U7XG4gICAgfVxuXG4gICAgY2xlYW5VcCAoKSB7XG4gICAgICAgIC8vIE5PVEU6IE9wdGlvbmFsLiBEbyBub3RoaW5nIGJ5IGRlZmF1bHQuXG4gICAgfVxufVxuIl19
