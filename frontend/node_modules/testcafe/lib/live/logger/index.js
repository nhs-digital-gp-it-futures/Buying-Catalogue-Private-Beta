'use strict';

exports.__esModule = true;
class Logger {
    constructor() {
        this.watching = true;

        this.MESSAGES = {
            intro: `
Live mode is enabled.
TestCafe now watches source files and reruns
the tests once the changes are saved.
                    
You can use the following keys in the terminal:
'Ctrl+S' - stops the test run;
'Ctrl+R' - restarts the test run;
'Ctrl+W' - enables/disables watching files;
'Ctrl+C' - quits live mode and closes the browsers.

`,

            sourceChanged: 'The sources have changed. A test run is starting...',
            testRunStarting: 'A test run is starting...',
            testRunStopping: 'The test run is stopping...',
            testRunFinishedWatching: 'Make changes to the source files or press Ctrl+R to restart the test run.',
            testRunFinishedNotWatching: 'Press Ctrl+R to restart the test run.',
            fileWatchingEnabled: 'TestCafe is watching the source files. Save the changes to run tests.',
            fileWatchingDisabled: 'TestCafe is not watching the source files.',
            nothingToStop: 'There are no tests running at the moment.',
            testCafeStopping: 'Stopping TestCafe live mode...',
            watchingFiles: 'Watching the following files:'
        };
    }

    _write(msg) {
        process.stdout.write(msg);
    }

    _status(msg) {
        this._write('\n' + msg + '\n');
    }

    writeIntroMessage(files) {
        this._write(this.MESSAGES.intro);

        if (!Array.isArray(files)) return;

        this._status(this.MESSAGES.watchingFiles);

        files.forEach(file => {
            this._write('  ' + file + '\n');
        });

        this._write('\n');
    }

    writeRunTestsMessage(sourcesChanged) {
        const statusMessage = sourcesChanged ? this.MESSAGES.sourceChanged : this.MESSAGES.testRunStarting;

        this._status(statusMessage);
    }

    writeTestsFinishedMessage() {
        const statusMessage = this.watching ? this.MESSAGES.testRunFinishedWatching : this.MESSAGES.testRunFinishedNotWatching;

        this._status(statusMessage);
    }

    writeStopRunningMessage() {
        this._status(this.MESSAGES.testRunStopping);
    }

    writeNothingToStopMessage() {
        this._status(this.MESSAGES.nothingToStop);
    }

    writeToggleWatchingMessage(enable) {
        this.watching = enable;

        const statusMessage = enable ? this.MESSAGES.fileWatchingEnabled : this.MESSAGES.fileWatchingDisabled;

        this._status(statusMessage);
    }

    writeExitMessage() {
        this._status(this.MESSAGES.testCafeStopping);
    }

    err(err) {
        /* eslint-disable no-console */
        console.log(err);
        /* eslint-enable no-console */
    }

}
exports.default = Logger;
module.exports = exports['default'];
//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIi4uLy4uLy4uL3NyYy9saXZlL2xvZ2dlci9pbmRleC5qcyJdLCJuYW1lcyI6WyJMb2dnZXIiLCJjb25zdHJ1Y3RvciIsIndhdGNoaW5nIiwiTUVTU0FHRVMiLCJpbnRybyIsInNvdXJjZUNoYW5nZWQiLCJ0ZXN0UnVuU3RhcnRpbmciLCJ0ZXN0UnVuU3RvcHBpbmciLCJ0ZXN0UnVuRmluaXNoZWRXYXRjaGluZyIsInRlc3RSdW5GaW5pc2hlZE5vdFdhdGNoaW5nIiwiZmlsZVdhdGNoaW5nRW5hYmxlZCIsImZpbGVXYXRjaGluZ0Rpc2FibGVkIiwibm90aGluZ1RvU3RvcCIsInRlc3RDYWZlU3RvcHBpbmciLCJ3YXRjaGluZ0ZpbGVzIiwiX3dyaXRlIiwibXNnIiwicHJvY2VzcyIsInN0ZG91dCIsIndyaXRlIiwiX3N0YXR1cyIsIndyaXRlSW50cm9NZXNzYWdlIiwiZmlsZXMiLCJBcnJheSIsImlzQXJyYXkiLCJmb3JFYWNoIiwiZmlsZSIsIndyaXRlUnVuVGVzdHNNZXNzYWdlIiwic291cmNlc0NoYW5nZWQiLCJzdGF0dXNNZXNzYWdlIiwid3JpdGVUZXN0c0ZpbmlzaGVkTWVzc2FnZSIsIndyaXRlU3RvcFJ1bm5pbmdNZXNzYWdlIiwid3JpdGVOb3RoaW5nVG9TdG9wTWVzc2FnZSIsIndyaXRlVG9nZ2xlV2F0Y2hpbmdNZXNzYWdlIiwiZW5hYmxlIiwid3JpdGVFeGl0TWVzc2FnZSIsImVyciIsImNvbnNvbGUiLCJsb2ciXSwibWFwcGluZ3MiOiI7OztBQUFlLE1BQU1BLE1BQU4sQ0FBYTtBQUN4QkMsa0JBQWU7QUFDWCxhQUFLQyxRQUFMLEdBQWdCLElBQWhCOztBQUVBLGFBQUtDLFFBQUwsR0FBZ0I7QUFDWkMsbUJBQVE7Ozs7Ozs7Ozs7O0NBREk7O0FBY1pDLDJCQUE0QixxREFkaEI7QUFlWkMsNkJBQTRCLDJCQWZoQjtBQWdCWkMsNkJBQTRCLDZCQWhCaEI7QUFpQlpDLHFDQUE0QiwyRUFqQmhCO0FBa0JaQyx3Q0FBNEIsdUNBbEJoQjtBQW1CWkMsaUNBQTRCLHVFQW5CaEI7QUFvQlpDLGtDQUE0Qiw0Q0FwQmhCO0FBcUJaQywyQkFBNEIsMkNBckJoQjtBQXNCWkMsOEJBQTRCLGdDQXRCaEI7QUF1QlpDLDJCQUE0QjtBQXZCaEIsU0FBaEI7QUF5Qkg7O0FBRURDLFdBQVFDLEdBQVIsRUFBYTtBQUNUQyxnQkFBUUMsTUFBUixDQUFlQyxLQUFmLENBQXFCSCxHQUFyQjtBQUNIOztBQUVESSxZQUFTSixHQUFULEVBQWM7QUFDVixhQUFLRCxNQUFMLENBQVksT0FBT0MsR0FBUCxHQUFhLElBQXpCO0FBQ0g7O0FBRURLLHNCQUFtQkMsS0FBbkIsRUFBMEI7QUFDdEIsYUFBS1AsTUFBTCxDQUFZLEtBQUtaLFFBQUwsQ0FBY0MsS0FBMUI7O0FBRUEsWUFBSSxDQUFDbUIsTUFBTUMsT0FBTixDQUFjRixLQUFkLENBQUwsRUFDSTs7QUFFSixhQUFLRixPQUFMLENBQWEsS0FBS2pCLFFBQUwsQ0FBY1csYUFBM0I7O0FBRUFRLGNBQU1HLE9BQU4sQ0FBY0MsUUFBUTtBQUNsQixpQkFBS1gsTUFBTCxDQUFZLE9BQU9XLElBQVAsR0FBYyxJQUExQjtBQUNILFNBRkQ7O0FBSUEsYUFBS1gsTUFBTCxDQUFZLElBQVo7QUFDSDs7QUFFRFkseUJBQXNCQyxjQUF0QixFQUFzQztBQUNsQyxjQUFNQyxnQkFBZ0JELGlCQUFpQixLQUFLekIsUUFBTCxDQUFjRSxhQUEvQixHQUErQyxLQUFLRixRQUFMLENBQWNHLGVBQW5GOztBQUVBLGFBQUtjLE9BQUwsQ0FBYVMsYUFBYjtBQUNIOztBQUVEQyxnQ0FBNkI7QUFDekIsY0FBTUQsZ0JBQWdCLEtBQUszQixRQUFMLEdBQWdCLEtBQUtDLFFBQUwsQ0FBY0ssdUJBQTlCLEdBQXdELEtBQUtMLFFBQUwsQ0FBY00sMEJBQTVGOztBQUVBLGFBQUtXLE9BQUwsQ0FBYVMsYUFBYjtBQUNIOztBQUVERSw4QkFBMkI7QUFDdkIsYUFBS1gsT0FBTCxDQUFhLEtBQUtqQixRQUFMLENBQWNJLGVBQTNCO0FBQ0g7O0FBRUR5QixnQ0FBNkI7QUFDekIsYUFBS1osT0FBTCxDQUFhLEtBQUtqQixRQUFMLENBQWNTLGFBQTNCO0FBQ0g7O0FBRURxQiwrQkFBNEJDLE1BQTVCLEVBQW9DO0FBQ2hDLGFBQUtoQyxRQUFMLEdBQWdCZ0MsTUFBaEI7O0FBRUEsY0FBTUwsZ0JBQWdCSyxTQUFTLEtBQUsvQixRQUFMLENBQWNPLG1CQUF2QixHQUE2QyxLQUFLUCxRQUFMLENBQWNRLG9CQUFqRjs7QUFFQSxhQUFLUyxPQUFMLENBQWFTLGFBQWI7QUFDSDs7QUFFRE0sdUJBQW9CO0FBQ2hCLGFBQUtmLE9BQUwsQ0FBYSxLQUFLakIsUUFBTCxDQUFjVSxnQkFBM0I7QUFDSDs7QUFFRHVCLFFBQUtBLEdBQUwsRUFBVTtBQUNOO0FBQ0FDLGdCQUFRQyxHQUFSLENBQVlGLEdBQVo7QUFDQTtBQUNIOztBQTFGdUI7a0JBQVBwQyxNIiwiZmlsZSI6ImxpdmUvbG9nZ2VyL2luZGV4LmpzIiwic291cmNlc0NvbnRlbnQiOlsiZXhwb3J0IGRlZmF1bHQgY2xhc3MgTG9nZ2VyIHtcbiAgICBjb25zdHJ1Y3RvciAoKSB7XG4gICAgICAgIHRoaXMud2F0Y2hpbmcgPSB0cnVlO1xuXG4gICAgICAgIHRoaXMuTUVTU0FHRVMgPSB7XG4gICAgICAgICAgICBpbnRybzogYFxuTGl2ZSBtb2RlIGlzIGVuYWJsZWQuXG5UZXN0Q2FmZSBub3cgd2F0Y2hlcyBzb3VyY2UgZmlsZXMgYW5kIHJlcnVuc1xudGhlIHRlc3RzIG9uY2UgdGhlIGNoYW5nZXMgYXJlIHNhdmVkLlxuICAgICAgICAgICAgICAgICAgICBcbllvdSBjYW4gdXNlIHRoZSBmb2xsb3dpbmcga2V5cyBpbiB0aGUgdGVybWluYWw6XG4nQ3RybCtTJyAtIHN0b3BzIHRoZSB0ZXN0IHJ1bjtcbidDdHJsK1InIC0gcmVzdGFydHMgdGhlIHRlc3QgcnVuO1xuJ0N0cmwrVycgLSBlbmFibGVzL2Rpc2FibGVzIHdhdGNoaW5nIGZpbGVzO1xuJ0N0cmwrQycgLSBxdWl0cyBsaXZlIG1vZGUgYW5kIGNsb3NlcyB0aGUgYnJvd3NlcnMuXG5cbmAsXG5cbiAgICAgICAgICAgIHNvdXJjZUNoYW5nZWQ6ICAgICAgICAgICAgICAnVGhlIHNvdXJjZXMgaGF2ZSBjaGFuZ2VkLiBBIHRlc3QgcnVuIGlzIHN0YXJ0aW5nLi4uJyxcbiAgICAgICAgICAgIHRlc3RSdW5TdGFydGluZzogICAgICAgICAgICAnQSB0ZXN0IHJ1biBpcyBzdGFydGluZy4uLicsXG4gICAgICAgICAgICB0ZXN0UnVuU3RvcHBpbmc6ICAgICAgICAgICAgJ1RoZSB0ZXN0IHJ1biBpcyBzdG9wcGluZy4uLicsXG4gICAgICAgICAgICB0ZXN0UnVuRmluaXNoZWRXYXRjaGluZzogICAgJ01ha2UgY2hhbmdlcyB0byB0aGUgc291cmNlIGZpbGVzIG9yIHByZXNzIEN0cmwrUiB0byByZXN0YXJ0IHRoZSB0ZXN0IHJ1bi4nLFxuICAgICAgICAgICAgdGVzdFJ1bkZpbmlzaGVkTm90V2F0Y2hpbmc6ICdQcmVzcyBDdHJsK1IgdG8gcmVzdGFydCB0aGUgdGVzdCBydW4uJyxcbiAgICAgICAgICAgIGZpbGVXYXRjaGluZ0VuYWJsZWQ6ICAgICAgICAnVGVzdENhZmUgaXMgd2F0Y2hpbmcgdGhlIHNvdXJjZSBmaWxlcy4gU2F2ZSB0aGUgY2hhbmdlcyB0byBydW4gdGVzdHMuJyxcbiAgICAgICAgICAgIGZpbGVXYXRjaGluZ0Rpc2FibGVkOiAgICAgICAnVGVzdENhZmUgaXMgbm90IHdhdGNoaW5nIHRoZSBzb3VyY2UgZmlsZXMuJyxcbiAgICAgICAgICAgIG5vdGhpbmdUb1N0b3A6ICAgICAgICAgICAgICAnVGhlcmUgYXJlIG5vIHRlc3RzIHJ1bm5pbmcgYXQgdGhlIG1vbWVudC4nLFxuICAgICAgICAgICAgdGVzdENhZmVTdG9wcGluZzogICAgICAgICAgICdTdG9wcGluZyBUZXN0Q2FmZSBsaXZlIG1vZGUuLi4nLFxuICAgICAgICAgICAgd2F0Y2hpbmdGaWxlczogICAgICAgICAgICAgICdXYXRjaGluZyB0aGUgZm9sbG93aW5nIGZpbGVzOicsXG4gICAgICAgIH07XG4gICAgfVxuXG4gICAgX3dyaXRlIChtc2cpIHtcbiAgICAgICAgcHJvY2Vzcy5zdGRvdXQud3JpdGUobXNnKTtcbiAgICB9XG5cbiAgICBfc3RhdHVzIChtc2cpIHtcbiAgICAgICAgdGhpcy5fd3JpdGUoJ1xcbicgKyBtc2cgKyAnXFxuJyk7XG4gICAgfVxuXG4gICAgd3JpdGVJbnRyb01lc3NhZ2UgKGZpbGVzKSB7XG4gICAgICAgIHRoaXMuX3dyaXRlKHRoaXMuTUVTU0FHRVMuaW50cm8pO1xuXG4gICAgICAgIGlmICghQXJyYXkuaXNBcnJheShmaWxlcykpXG4gICAgICAgICAgICByZXR1cm47XG5cbiAgICAgICAgdGhpcy5fc3RhdHVzKHRoaXMuTUVTU0FHRVMud2F0Y2hpbmdGaWxlcyk7XG5cbiAgICAgICAgZmlsZXMuZm9yRWFjaChmaWxlID0+IHtcbiAgICAgICAgICAgIHRoaXMuX3dyaXRlKCcgICcgKyBmaWxlICsgJ1xcbicpO1xuICAgICAgICB9KTtcblxuICAgICAgICB0aGlzLl93cml0ZSgnXFxuJyk7XG4gICAgfVxuXG4gICAgd3JpdGVSdW5UZXN0c01lc3NhZ2UgKHNvdXJjZXNDaGFuZ2VkKSB7XG4gICAgICAgIGNvbnN0IHN0YXR1c01lc3NhZ2UgPSBzb3VyY2VzQ2hhbmdlZCA/IHRoaXMuTUVTU0FHRVMuc291cmNlQ2hhbmdlZCA6IHRoaXMuTUVTU0FHRVMudGVzdFJ1blN0YXJ0aW5nO1xuXG4gICAgICAgIHRoaXMuX3N0YXR1cyhzdGF0dXNNZXNzYWdlKTtcbiAgICB9XG5cbiAgICB3cml0ZVRlc3RzRmluaXNoZWRNZXNzYWdlICgpIHtcbiAgICAgICAgY29uc3Qgc3RhdHVzTWVzc2FnZSA9IHRoaXMud2F0Y2hpbmcgPyB0aGlzLk1FU1NBR0VTLnRlc3RSdW5GaW5pc2hlZFdhdGNoaW5nIDogdGhpcy5NRVNTQUdFUy50ZXN0UnVuRmluaXNoZWROb3RXYXRjaGluZztcblxuICAgICAgICB0aGlzLl9zdGF0dXMoc3RhdHVzTWVzc2FnZSk7XG4gICAgfVxuXG4gICAgd3JpdGVTdG9wUnVubmluZ01lc3NhZ2UgKCkge1xuICAgICAgICB0aGlzLl9zdGF0dXModGhpcy5NRVNTQUdFUy50ZXN0UnVuU3RvcHBpbmcpO1xuICAgIH1cblxuICAgIHdyaXRlTm90aGluZ1RvU3RvcE1lc3NhZ2UgKCkge1xuICAgICAgICB0aGlzLl9zdGF0dXModGhpcy5NRVNTQUdFUy5ub3RoaW5nVG9TdG9wKTtcbiAgICB9XG5cbiAgICB3cml0ZVRvZ2dsZVdhdGNoaW5nTWVzc2FnZSAoZW5hYmxlKSB7XG4gICAgICAgIHRoaXMud2F0Y2hpbmcgPSBlbmFibGU7XG5cbiAgICAgICAgY29uc3Qgc3RhdHVzTWVzc2FnZSA9IGVuYWJsZSA/IHRoaXMuTUVTU0FHRVMuZmlsZVdhdGNoaW5nRW5hYmxlZCA6IHRoaXMuTUVTU0FHRVMuZmlsZVdhdGNoaW5nRGlzYWJsZWQ7XG5cbiAgICAgICAgdGhpcy5fc3RhdHVzKHN0YXR1c01lc3NhZ2UpO1xuICAgIH1cblxuICAgIHdyaXRlRXhpdE1lc3NhZ2UgKCkge1xuICAgICAgICB0aGlzLl9zdGF0dXModGhpcy5NRVNTQUdFUy50ZXN0Q2FmZVN0b3BwaW5nKTtcbiAgICB9XG5cbiAgICBlcnIgKGVycikge1xuICAgICAgICAvKiBlc2xpbnQtZGlzYWJsZSBuby1jb25zb2xlICovXG4gICAgICAgIGNvbnNvbGUubG9nKGVycik7XG4gICAgICAgIC8qIGVzbGludC1lbmFibGUgbm8tY29uc29sZSAqL1xuICAgIH1cblxuXG59XG4iXX0=
