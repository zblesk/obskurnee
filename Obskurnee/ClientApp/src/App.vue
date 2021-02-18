<template>
  <nav-menu></nav-menu>
  <router-view />
</template>

<script>
import { mapActions } from 'vuex'
import NavMenu from './components/NavMenu.vue'

export default {
  name: 'App',
  components: {
      NavMenu
  },
  created () {
    this.restoreContext()
  },
  methods: {
    ...mapActions('context', [
      'restoreContext'
    ])
  }
}
</script>

<style>

.todo {
  margin: 2em;
  color: orangered;
  text-align: center;
}

/* variables */
:root {
  --spacer: 20px;
  --c-font: #292929;
  --c-font-rev: #FFFFFF;
  --c-accent: #79AA2D;
  --c-accent-darker: #648d25;
  --c-accent-secondary: #999999;
  --c-bckgr-primary: #FFFFFF;
  --c-bckgr-secondary: #F5F5F5;
}

html {
  -webkit-box-sizing: border-box;
          box-sizing: border-box;
  }

*, :after, :before {
  -webkit-box-sizing: inherit;
          box-sizing: inherit;
  margin: 0;
  padding: 0;
}

body {
  background-color: var(--c-bckgr-secondary);
}

#app {
  font-family: 'Raleway', Helvetica, Arial, sans-serif;
  font-size: 100%;
  color: var(--c-font);

  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  margin-top: 60px;
}

.page-title {
  font-size: 2em;
  font-weight: bold;
  text-align: center;
  margin-top: calc(1.5 * var(--spacer));
  margin-bottom: var(--spacer);
}

.button-primary,
.button-secondary {
  border-radius: 4px;
  font-size: 1em;
  font-weight: bold;
  padding: 0.625em 1.25em;
  cursor: pointer;
  display: inline-block;
}

.button-primary {
  border: 2px solid var(--c-accent);
  background-color: var(--c-accent);
  color: var(--c-font-rev);
}

.button-primary:hover {
  color: var(--c-font-rev);
  text-decoration: none;
  background-color: var(--c-accent-darker);
  border-color: var(--c-accent-darker);
}

.button-secondary {
  border: 2px solid var(--c-accent-secondary);
  background-color:var(--c-bckgr-primary);
  color: var(--c-accent-secondary);
}

.button-secondary:hover {
  color: var(--c-font-rev);
  text-decoration: none;
  background-color: var(--c-accent-secondary);
}

/* generic styling for markdown */
p {
  font-size: 1em;
  color: var(--c-font);
  margin-bottom: 1em;
}

h1, h2, h3 {
  color: var(--c-font);
  font-weight: bold;
}

h1 {
  font-size: 2em;
  margin-top: 0.67em;
  margin-bottom: 0.67em;
}

h2 {
  font-size: 1.5em;
  margin-top: 0.83em;
  margin-bottom: 0.83em;
}

h3 {
  font-size: 1.17em;
  margin-top: 1em;
  margin-bottom: 1em;
}

blockquote {
  margin-left: var(--spacer);
  font-style: italic;
}

ul,
ol {
  margin-bottom: 1em;
  padding-left: calc(2 * var(--spacer));
}

li {
  margin-bottom: 0.625em;
}

img {
  max-width: 100%;
}

hr {
  margin-top: var(--spacer);
  margin-bottom: var(--spacer);
}

a {
  color: var(--c-accent);
}

a:hover {
  color: var(--c-accent-darker);
}

</style>
