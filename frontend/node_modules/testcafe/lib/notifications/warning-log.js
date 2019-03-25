'use strict';

exports.__esModule = true;

var _renderTemplate = require('../utils/render-template');

var _renderTemplate2 = _interopRequireDefault(_renderTemplate);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

class WarningLog {
    constructor(globalLog = null) {
        this.globalLog = globalLog;
        this.messages = [];
    }

    addPlainMessage(msg) {
        // NOTE: avoid duplicates
        if (this.messages.indexOf(msg) < 0) this.messages.push(msg);
    }

    addWarning() {
        const msg = _renderTemplate2.default.apply(null, arguments);

        this.addPlainMessage(msg);

        if (this.globalLog) this.globalLog.addPlainMessage(msg);
    }
}
exports.default = WarningLog;
module.exports = exports['default'];
//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIi4uLy4uL3NyYy9ub3RpZmljYXRpb25zL3dhcm5pbmctbG9nLmpzIl0sIm5hbWVzIjpbIldhcm5pbmdMb2ciLCJjb25zdHJ1Y3RvciIsImdsb2JhbExvZyIsIm1lc3NhZ2VzIiwiYWRkUGxhaW5NZXNzYWdlIiwibXNnIiwiaW5kZXhPZiIsInB1c2giLCJhZGRXYXJuaW5nIiwicmVuZGVyVGVtcGxhdGUiLCJhcHBseSIsImFyZ3VtZW50cyJdLCJtYXBwaW5ncyI6Ijs7OztBQUFBOzs7Ozs7QUFFZSxNQUFNQSxVQUFOLENBQWlCO0FBQzVCQyxnQkFBYUMsWUFBWSxJQUF6QixFQUErQjtBQUMzQixhQUFLQSxTQUFMLEdBQWlCQSxTQUFqQjtBQUNBLGFBQUtDLFFBQUwsR0FBaUIsRUFBakI7QUFDSDs7QUFFREMsb0JBQWlCQyxHQUFqQixFQUFzQjtBQUNsQjtBQUNBLFlBQUksS0FBS0YsUUFBTCxDQUFjRyxPQUFkLENBQXNCRCxHQUF0QixJQUE2QixDQUFqQyxFQUNJLEtBQUtGLFFBQUwsQ0FBY0ksSUFBZCxDQUFtQkYsR0FBbkI7QUFDUDs7QUFFREcsaUJBQWM7QUFDVixjQUFNSCxNQUFNSSx5QkFBZUMsS0FBZixDQUFxQixJQUFyQixFQUEyQkMsU0FBM0IsQ0FBWjs7QUFFQSxhQUFLUCxlQUFMLENBQXFCQyxHQUFyQjs7QUFFQSxZQUFJLEtBQUtILFNBQVQsRUFDSSxLQUFLQSxTQUFMLENBQWVFLGVBQWYsQ0FBK0JDLEdBQS9CO0FBQ1A7QUFuQjJCO2tCQUFYTCxVIiwiZmlsZSI6Im5vdGlmaWNhdGlvbnMvd2FybmluZy1sb2cuanMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgcmVuZGVyVGVtcGxhdGUgZnJvbSAnLi4vdXRpbHMvcmVuZGVyLXRlbXBsYXRlJztcblxuZXhwb3J0IGRlZmF1bHQgY2xhc3MgV2FybmluZ0xvZyB7XG4gICAgY29uc3RydWN0b3IgKGdsb2JhbExvZyA9IG51bGwpIHtcbiAgICAgICAgdGhpcy5nbG9iYWxMb2cgPSBnbG9iYWxMb2c7XG4gICAgICAgIHRoaXMubWVzc2FnZXMgID0gW107XG4gICAgfVxuXG4gICAgYWRkUGxhaW5NZXNzYWdlIChtc2cpIHtcbiAgICAgICAgLy8gTk9URTogYXZvaWQgZHVwbGljYXRlc1xuICAgICAgICBpZiAodGhpcy5tZXNzYWdlcy5pbmRleE9mKG1zZykgPCAwKVxuICAgICAgICAgICAgdGhpcy5tZXNzYWdlcy5wdXNoKG1zZyk7XG4gICAgfVxuXG4gICAgYWRkV2FybmluZyAoKSB7XG4gICAgICAgIGNvbnN0IG1zZyA9IHJlbmRlclRlbXBsYXRlLmFwcGx5KG51bGwsIGFyZ3VtZW50cyk7XG5cbiAgICAgICAgdGhpcy5hZGRQbGFpbk1lc3NhZ2UobXNnKTtcblxuICAgICAgICBpZiAodGhpcy5nbG9iYWxMb2cpXG4gICAgICAgICAgICB0aGlzLmdsb2JhbExvZy5hZGRQbGFpbk1lc3NhZ2UobXNnKTtcbiAgICB9XG59XG4iXX0=
