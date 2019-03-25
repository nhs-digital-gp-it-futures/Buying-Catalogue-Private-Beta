'use strict';

exports.__esModule = true;

var _getOwnPropertyNames = require('babel-runtime/core-js/object/get-own-property-names');

var _getOwnPropertyNames2 = _interopRequireDefault(_getOwnPropertyNames);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

class FlagList {
    constructor({ initialFlagValue, flags }) {
        Object.defineProperty(this, '_initialFlagValue', { writable: true, value: initialFlagValue });

        flags.forEach(flag => {
            this[flag] = initialFlagValue;
        });
    }

    reset() {
        (0, _getOwnPropertyNames2.default)(this).forEach(name => {
            this[name] = !this._initialFlagValue;
        });
    }
}
exports.default = FlagList;
module.exports = exports['default'];
//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIi4uLy4uL3NyYy91dGlscy9mbGFnLWxpc3QuanMiXSwibmFtZXMiOlsiRmxhZ0xpc3QiLCJjb25zdHJ1Y3RvciIsImluaXRpYWxGbGFnVmFsdWUiLCJmbGFncyIsIk9iamVjdCIsImRlZmluZVByb3BlcnR5Iiwid3JpdGFibGUiLCJ2YWx1ZSIsImZvckVhY2giLCJmbGFnIiwicmVzZXQiLCJuYW1lIiwiX2luaXRpYWxGbGFnVmFsdWUiXSwibWFwcGluZ3MiOiI7Ozs7Ozs7Ozs7QUFBZSxNQUFNQSxRQUFOLENBQWU7QUFDMUJDLGdCQUFhLEVBQUVDLGdCQUFGLEVBQW9CQyxLQUFwQixFQUFiLEVBQTBDO0FBQ3RDQyxlQUFPQyxjQUFQLENBQXNCLElBQXRCLEVBQTRCLG1CQUE1QixFQUFpRCxFQUFFQyxVQUFVLElBQVosRUFBa0JDLE9BQU9MLGdCQUF6QixFQUFqRDs7QUFFQUMsY0FBTUssT0FBTixDQUFjQyxRQUFRO0FBQ2xCLGlCQUFLQSxJQUFMLElBQWFQLGdCQUFiO0FBQ0gsU0FGRDtBQUdIOztBQUVEUSxZQUFTO0FBQ0wsMkNBQTJCLElBQTNCLEVBQ0tGLE9BREwsQ0FDYUcsUUFBUTtBQUNiLGlCQUFLQSxJQUFMLElBQWEsQ0FBQyxLQUFLQyxpQkFBbkI7QUFDSCxTQUhMO0FBSUg7QUFkeUI7a0JBQVRaLFEiLCJmaWxlIjoidXRpbHMvZmxhZy1saXN0LmpzIiwic291cmNlc0NvbnRlbnQiOlsiZXhwb3J0IGRlZmF1bHQgY2xhc3MgRmxhZ0xpc3Qge1xuICAgIGNvbnN0cnVjdG9yICh7IGluaXRpYWxGbGFnVmFsdWUsIGZsYWdzIH0pIHtcbiAgICAgICAgT2JqZWN0LmRlZmluZVByb3BlcnR5KHRoaXMsICdfaW5pdGlhbEZsYWdWYWx1ZScsIHsgd3JpdGFibGU6IHRydWUsIHZhbHVlOiBpbml0aWFsRmxhZ1ZhbHVlIH0pO1xuXG4gICAgICAgIGZsYWdzLmZvckVhY2goZmxhZyA9PiB7XG4gICAgICAgICAgICB0aGlzW2ZsYWddID0gaW5pdGlhbEZsYWdWYWx1ZTtcbiAgICAgICAgfSk7XG4gICAgfVxuXG4gICAgcmVzZXQgKCkge1xuICAgICAgICBPYmplY3QuZ2V0T3duUHJvcGVydHlOYW1lcyh0aGlzKVxuICAgICAgICAgICAgLmZvckVhY2gobmFtZSA9PiB7XG4gICAgICAgICAgICAgICAgdGhpc1tuYW1lXSA9ICF0aGlzLl9pbml0aWFsRmxhZ1ZhbHVlO1xuICAgICAgICAgICAgfSk7XG4gICAgfVxufVxuIl19
