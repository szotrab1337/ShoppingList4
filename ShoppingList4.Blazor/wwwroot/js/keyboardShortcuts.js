window.keyboardShortcuts = {
    shortcuts: {}, // Object to store registered shortcuts

    registerShortcut: function (key, dotNetReference) {
        // Store the reference to the callback function
        window.keyboardShortcuts.shortcuts[key] = dotNetReference;

        // Add event listener for the shortcut key
        document.addEventListener('keydown', window.keyboardShortcuts.handleKeyDown);
    },

    unregisterShortcut: function (key) {
        // Remove the stored callback function reference
        delete window.keyboardShortcuts.shortcuts[key];

        // Check if there are any shortcuts left
        if (Object.keys(window.keyboardShortcuts.shortcuts).length === 0) {
            // If no shortcuts left, remove the event listener
            document.removeEventListener('keydown', window.keyboardShortcuts.handleKeyDown);
        }
    },

    handleKeyDown: function (event) {
        // Check if the pressed key is registered as a shortcut
        if (window.keyboardShortcuts.shortcuts[event.key]) {
            // If yes, invoke the corresponding .NET callback function
            window.keyboardShortcuts.shortcuts[event.key].invokeMethodAsync('InvokeShortcut');
            event.preventDefault(); // Prevent default browser behavior for the shortcut key
        }
    }
};