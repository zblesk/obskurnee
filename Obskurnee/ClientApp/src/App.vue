<template>
<div class="page-container">
  <nav-menu></nav-menu>
  <div class="page-center">
    <router-view />
  </div>
  <footer class="footer">
    <p class="attribution">Icons made by <a href="https://www.freepik.com" title="Freepik">Freepik</a> and <a href="https://www.flaticon.com/authors/pixel-perfect" title="Pixel perfect">Pixel perfect</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a>.</p>
  </footer>
</div>
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

.todo-l {
  margin: 2em;
  color: darkred;
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
  text-decoration: none;
  padding: 0.625em 1.25em;
  cursor: pointer;
  display: inline-block;
}

.button-primary {
  border: 2px solid var(--c-accent);
  background-color: var(--c-accent);
  color: var(--c-font-rev);
}

.button-primary:hover,
.button-primary:focus,
.button-primary:active {
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

.button-secondary:hover,
.button-secondary:focus,
.button-secondary:active {
  color: var(--c-font-rev);
  text-decoration: none;
  background-color: var(--c-accent-secondary);
}

textarea,
input {
  font-size: 14px;
  padding: calc(var(--spacer) / 2);
  outline: none;
}

/* generic styling for markdown */

p {
  font-size: 1em;
  line-height: 1.5;
  color: var(--c-font);
  margin-bottom: 1em;
}

p:last-child {
  margin-bottom: 0;
}

.book__pitch p {
  font-size: 0.875em;
}

.book__pitch h2 {
  font-size: 1em;
  margin: 0 0 0.5em 0;
}

h1, h2, h3 {
  color: var(--c-font);
  font-weight: bold;
  margin-top: 0;
}

a {
  color: var(--c-accent);
}

a:hover,
a:focus,
a:active {
  color: var(--c-accent-darker);
}

h1 {
  font-size: 1.5em;
  margin-bottom: var(--spacer);
}

h2 {
  font-size: 1.2em;
  margin-bottom: var(--spacer);
}

h3 {
  font-size: 1.17em;
  margin-bottom: var(--spacer);
}

blockquote {
  margin-left: var(--spacer);
  font-style: italic;
  margin-bottom: var(--spacer);
}

ul,
ol {
  margin-bottom: var(--spacer);
  padding-left: calc(2 * var(--spacer));
}

li {
  margin-bottom: calc(var(--spacer) / 2);
}

img {
  max-width: 100%;
}

hr {
  margin-top: var(--spacer);
  margin-bottom: var(--spacer);
}

/* footer */
.footer {
  background-color: var(--c-bckgr-primary);
  padding: var(--spacer);
}

.attribution {
  font-size: 0.9em;
  text-align: center;
}

.attribution a {
  color: var(--c-accent);
}

.page-container {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.page-center {
  flex: 1 0 100%;
}

</style>
