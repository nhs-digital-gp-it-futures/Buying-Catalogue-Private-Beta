'use strict';

exports.__esModule = true;

var _path = require('path');

var _path2 = _interopRequireDefault(_path);

var _module = require('module');

var _module2 = _interopRequireDefault(_module);

var _bootstrapper = require('../runner/bootstrapper');

var _bootstrapper2 = _interopRequireDefault(_bootstrapper);

var _compiler = require('../compiler');

var _compiler2 = _interopRequireDefault(_compiler);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

const originalRequire = _module2.default.prototype.require;

class LiveModeBootstrapper extends _bootstrapper2.default {
    constructor(runner, browserConnectionGateway) {
        super(browserConnectionGateway);

        this.runner = runner;
    }

    _getTests() {
        this._mockRequire();

        return super._getTests().then(result => {
            this._restoreRequire();

            return result;
        }).catch(err => {
            this._restoreRequire();

            _compiler2.default.cleanUp();

            this.runner.setBootstrappingError(err);
        });
    }

    _mockRequire() {
        const runner = this.runner;

        // NODE: we replace the `require` method to add required files to watcher
        _module2.default.prototype.require = function (filePath) {
            const filename = _module2.default._resolveFilename(filePath, this, false);

            if (_path2.default.isAbsolute(filename) || /^\.\.?[/\\]/.test(filename)) runner.emit(runner.REQUIRED_MODULE_FOUND_EVENT, { filename });

            return originalRequire.apply(this, arguments);
        };
    }

    _restoreRequire() {
        _module2.default.prototype.require = originalRequire;
    }
}

exports.default = LiveModeBootstrapper;
module.exports = exports['default'];
//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIi4uLy4uL3NyYy9saXZlL2Jvb3RzdHJhcHBlci5qcyJdLCJuYW1lcyI6WyJvcmlnaW5hbFJlcXVpcmUiLCJNb2R1bGUiLCJwcm90b3R5cGUiLCJyZXF1aXJlIiwiTGl2ZU1vZGVCb290c3RyYXBwZXIiLCJCb290c3RyYXBwZXIiLCJjb25zdHJ1Y3RvciIsInJ1bm5lciIsImJyb3dzZXJDb25uZWN0aW9uR2F0ZXdheSIsIl9nZXRUZXN0cyIsIl9tb2NrUmVxdWlyZSIsInRoZW4iLCJyZXN1bHQiLCJfcmVzdG9yZVJlcXVpcmUiLCJjYXRjaCIsImVyciIsIkNvbXBpbGVyIiwiY2xlYW5VcCIsInNldEJvb3RzdHJhcHBpbmdFcnJvciIsImZpbGVQYXRoIiwiZmlsZW5hbWUiLCJfcmVzb2x2ZUZpbGVuYW1lIiwicGF0aCIsImlzQWJzb2x1dGUiLCJ0ZXN0IiwiZW1pdCIsIlJFUVVJUkVEX01PRFVMRV9GT1VORF9FVkVOVCIsImFwcGx5IiwiYXJndW1lbnRzIl0sIm1hcHBpbmdzIjoiOzs7O0FBQUE7Ozs7QUFDQTs7OztBQUNBOzs7O0FBQ0E7Ozs7OztBQUVBLE1BQU1BLGtCQUFrQkMsaUJBQU9DLFNBQVAsQ0FBaUJDLE9BQXpDOztBQUVBLE1BQU1DLG9CQUFOLFNBQW1DQyxzQkFBbkMsQ0FBZ0Q7QUFDNUNDLGdCQUFhQyxNQUFiLEVBQXFCQyx3QkFBckIsRUFBK0M7QUFDM0MsY0FBTUEsd0JBQU47O0FBRUEsYUFBS0QsTUFBTCxHQUFjQSxNQUFkO0FBQ0g7O0FBRURFLGdCQUFhO0FBQ1QsYUFBS0MsWUFBTDs7QUFFQSxlQUFPLE1BQU1ELFNBQU4sR0FDRkUsSUFERSxDQUNHQyxVQUFVO0FBQ1osaUJBQUtDLGVBQUw7O0FBRUEsbUJBQU9ELE1BQVA7QUFDSCxTQUxFLEVBTUZFLEtBTkUsQ0FNSUMsT0FBTztBQUNWLGlCQUFLRixlQUFMOztBQUVBRywrQkFBU0MsT0FBVDs7QUFFQSxpQkFBS1YsTUFBTCxDQUFZVyxxQkFBWixDQUFrQ0gsR0FBbEM7QUFDSCxTQVpFLENBQVA7QUFhSDs7QUFFREwsbUJBQWdCO0FBQ1osY0FBTUgsU0FBUyxLQUFLQSxNQUFwQjs7QUFFQTtBQUNBTix5QkFBT0MsU0FBUCxDQUFpQkMsT0FBakIsR0FBMkIsVUFBVWdCLFFBQVYsRUFBb0I7QUFDM0Msa0JBQU1DLFdBQVduQixpQkFBT29CLGdCQUFQLENBQXdCRixRQUF4QixFQUFrQyxJQUFsQyxFQUF3QyxLQUF4QyxDQUFqQjs7QUFFQSxnQkFBSUcsZUFBS0MsVUFBTCxDQUFnQkgsUUFBaEIsS0FBNkIsY0FBY0ksSUFBZCxDQUFtQkosUUFBbkIsQ0FBakMsRUFDSWIsT0FBT2tCLElBQVAsQ0FBWWxCLE9BQU9tQiwyQkFBbkIsRUFBZ0QsRUFBRU4sUUFBRixFQUFoRDs7QUFHSixtQkFBT3BCLGdCQUFnQjJCLEtBQWhCLENBQXNCLElBQXRCLEVBQTRCQyxTQUE1QixDQUFQO0FBQ0gsU0FSRDtBQVNIOztBQUVEZixzQkFBbUI7QUFDZloseUJBQU9DLFNBQVAsQ0FBaUJDLE9BQWpCLEdBQTJCSCxlQUEzQjtBQUNIO0FBMUMyQzs7a0JBNkNqQ0ksb0IiLCJmaWxlIjoibGl2ZS9ib290c3RyYXBwZXIuanMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgcGF0aCBmcm9tICdwYXRoJztcbmltcG9ydCBNb2R1bGUgZnJvbSAnbW9kdWxlJztcbmltcG9ydCBCb290c3RyYXBwZXIgZnJvbSAnLi4vcnVubmVyL2Jvb3RzdHJhcHBlcic7XG5pbXBvcnQgQ29tcGlsZXIgZnJvbSAnLi4vY29tcGlsZXInO1xuXG5jb25zdCBvcmlnaW5hbFJlcXVpcmUgPSBNb2R1bGUucHJvdG90eXBlLnJlcXVpcmU7XG5cbmNsYXNzIExpdmVNb2RlQm9vdHN0cmFwcGVyIGV4dGVuZHMgQm9vdHN0cmFwcGVyIHtcbiAgICBjb25zdHJ1Y3RvciAocnVubmVyLCBicm93c2VyQ29ubmVjdGlvbkdhdGV3YXkpIHtcbiAgICAgICAgc3VwZXIoYnJvd3NlckNvbm5lY3Rpb25HYXRld2F5KTtcblxuICAgICAgICB0aGlzLnJ1bm5lciA9IHJ1bm5lcjtcbiAgICB9XG5cbiAgICBfZ2V0VGVzdHMgKCkge1xuICAgICAgICB0aGlzLl9tb2NrUmVxdWlyZSgpO1xuXG4gICAgICAgIHJldHVybiBzdXBlci5fZ2V0VGVzdHMoKVxuICAgICAgICAgICAgLnRoZW4ocmVzdWx0ID0+IHtcbiAgICAgICAgICAgICAgICB0aGlzLl9yZXN0b3JlUmVxdWlyZSgpO1xuXG4gICAgICAgICAgICAgICAgcmV0dXJuIHJlc3VsdDtcbiAgICAgICAgICAgIH0pXG4gICAgICAgICAgICAuY2F0Y2goZXJyID0+IHtcbiAgICAgICAgICAgICAgICB0aGlzLl9yZXN0b3JlUmVxdWlyZSgpO1xuXG4gICAgICAgICAgICAgICAgQ29tcGlsZXIuY2xlYW5VcCgpO1xuXG4gICAgICAgICAgICAgICAgdGhpcy5ydW5uZXIuc2V0Qm9vdHN0cmFwcGluZ0Vycm9yKGVycik7XG4gICAgICAgICAgICB9KTtcbiAgICB9XG5cbiAgICBfbW9ja1JlcXVpcmUgKCkge1xuICAgICAgICBjb25zdCBydW5uZXIgPSB0aGlzLnJ1bm5lcjtcblxuICAgICAgICAvLyBOT0RFOiB3ZSByZXBsYWNlIHRoZSBgcmVxdWlyZWAgbWV0aG9kIHRvIGFkZCByZXF1aXJlZCBmaWxlcyB0byB3YXRjaGVyXG4gICAgICAgIE1vZHVsZS5wcm90b3R5cGUucmVxdWlyZSA9IGZ1bmN0aW9uIChmaWxlUGF0aCkge1xuICAgICAgICAgICAgY29uc3QgZmlsZW5hbWUgPSBNb2R1bGUuX3Jlc29sdmVGaWxlbmFtZShmaWxlUGF0aCwgdGhpcywgZmFsc2UpO1xuXG4gICAgICAgICAgICBpZiAocGF0aC5pc0Fic29sdXRlKGZpbGVuYW1lKSB8fCAvXlxcLlxcLj9bL1xcXFxdLy50ZXN0KGZpbGVuYW1lKSlcbiAgICAgICAgICAgICAgICBydW5uZXIuZW1pdChydW5uZXIuUkVRVUlSRURfTU9EVUxFX0ZPVU5EX0VWRU5ULCB7IGZpbGVuYW1lIH0pO1xuXG5cbiAgICAgICAgICAgIHJldHVybiBvcmlnaW5hbFJlcXVpcmUuYXBwbHkodGhpcywgYXJndW1lbnRzKTtcbiAgICAgICAgfTtcbiAgICB9XG5cbiAgICBfcmVzdG9yZVJlcXVpcmUgKCkge1xuICAgICAgICBNb2R1bGUucHJvdG90eXBlLnJlcXVpcmUgPSBvcmlnaW5hbFJlcXVpcmU7XG4gICAgfVxufVxuXG5leHBvcnQgZGVmYXVsdCBMaXZlTW9kZUJvb3RzdHJhcHBlcjtcbiJdfQ==
