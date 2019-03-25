'use strict';

exports.__esModule = true;

var _optionSource = require('./option-source');

var _optionSource2 = _interopRequireDefault(_optionSource);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

class Option {
    constructor(name, value, source = _optionSource2.default.configuration) {
        this.name = name;
        this.value = value;
        this.source = source;
    }
}
exports.default = Option;
module.exports = exports['default'];
//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIi4uLy4uL3NyYy9jb25maWd1cmF0aW9uL29wdGlvbi5qcyJdLCJuYW1lcyI6WyJPcHRpb24iLCJjb25zdHJ1Y3RvciIsIm5hbWUiLCJ2YWx1ZSIsInNvdXJjZSIsIm9wdGlvblNvdXJjZSIsImNvbmZpZ3VyYXRpb24iXSwibWFwcGluZ3MiOiI7Ozs7QUFBQTs7Ozs7O0FBRWUsTUFBTUEsTUFBTixDQUFhO0FBQ3hCQyxnQkFBYUMsSUFBYixFQUFtQkMsS0FBbkIsRUFBMEJDLFNBQVNDLHVCQUFhQyxhQUFoRCxFQUErRDtBQUMzRCxhQUFLSixJQUFMLEdBQWNBLElBQWQ7QUFDQSxhQUFLQyxLQUFMLEdBQWNBLEtBQWQ7QUFDQSxhQUFLQyxNQUFMLEdBQWNBLE1BQWQ7QUFDSDtBQUx1QjtrQkFBUEosTSIsImZpbGUiOiJjb25maWd1cmF0aW9uL29wdGlvbi5qcyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCBvcHRpb25Tb3VyY2UgZnJvbSAnLi9vcHRpb24tc291cmNlJztcblxuZXhwb3J0IGRlZmF1bHQgY2xhc3MgT3B0aW9uIHtcbiAgICBjb25zdHJ1Y3RvciAobmFtZSwgdmFsdWUsIHNvdXJjZSA9IG9wdGlvblNvdXJjZS5jb25maWd1cmF0aW9uKSB7XG4gICAgICAgIHRoaXMubmFtZSAgID0gbmFtZTtcbiAgICAgICAgdGhpcy52YWx1ZSAgPSB2YWx1ZTtcbiAgICAgICAgdGhpcy5zb3VyY2UgPSBzb3VyY2U7XG4gICAgfVxufVxuIl19
