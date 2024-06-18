module.exports = {
    env: {
        browser: true,
        es2021: true,
        node: true, // Ensure Node environment is specified
    },
    extends: [
        'eslint:recommended',
        'plugin:react/recommended',
        'plugin:node/recommended',
    ],
    parserOptions: {
        ecmaVersion: 12, // Ensure ECMAScript 12 or later
        sourceType: 'module',
    },
    rules: {
        'no-process-exit': 'error', // Ensure this rule is enforced
        'node/no-unpublished-import': ['error', {
            allowModules: ['vite', '@vitejs/plugin-react']
        }],
        // Your custom rules
    },
};
