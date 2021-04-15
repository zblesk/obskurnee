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

/* centre piece of pages */
/* used in: admin, discussion, passwordReset, poll, recommendationList, roundList, setup, user */

.main {
  max-width: 800px;
  background-color: var(--c-bckgr-primary);
  margin: var(--spacer);
  padding: calc(2* var(--spacer));
  text-align: left;
}

@media screen and (min-width: 840px) {
  .main {
    margin: var(--spacer) auto;
  }
}

/* grid */
/* used in: book, discussion, recommendationList, user */
.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(150px, 450px));
  gap: var(--spacer);
  margin: 0;
  padding: var(--spacer);
}

/* buttons */
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

/* buttons - layout */
/* used in: book, discussion, newPost, poll, roundList, user */
.buttons {
  display: flex;
  flex-direction: column;
  text-align: center;
}

@media screen and (min-width: 576px) {
  .buttons {
    flex-direction: row;
    justify-content: center;
  }
}

/* note */
/* used in: admin, poll, newPost */
.note {
  display: flex;
  align-items: center;
  padding: 0 calc(var(--spacer) / 2);
}

.note-pic {
  width: 20px;
  flex-shrink: 0;
}

.note-pic img {
  width: 100%;
}

.note-text {
  font-size: 0.875em;
  opacity: 0.8;
  margin: 0 0 0 calc(var(--spacer) / 2);
}

/* forms */
/* used in: admin, newPost, passwordReset, roundList, setup, user */

.form-field {
  margin-bottom: var(--spacer);
}

.form-field label {
  display: block;
  font-size: 1em;
  margin-bottom: calc(var(--spacer) / 3);
}

.form-field input {
  font-size: 0.875em;
  width: 100%;
  border: 0;
  background-color: var(--c-bckgr-secondary);
  padding: calc(var(--spacer) / 2);
}

.form-field textarea {
  font-size: 0.875em;
  border: 0;
  background-color: var(--c-bckgr-secondary);
  padding: calc(var(--spacer) / 2);
  width: 100%;
  height: 15em;
}

/* label with markdown help */
.label-md-wrapper {
  display: flex;
  flex-direction: column;
  margin-bottom: calc(var(--spacer) / 3);
}

@media screen and (min-width: 576px) {
  .label-md-wrapper {
    flex-direction: row;
    justify-content: space-between;
  }

  .label-md-wrapper .label-md {
    margin-bottom: 0;
  }

  .label-md-wrapper .mo-md {
    margin-bottom: 0;
  }
}

.mo-md {
  display: flex;
  align-items: center;
  padding: 0;
  margin-bottom: calc(var(--spacer) / 2);
}

.mo-md-pic {
  width: 26px;
  flex-shrink: 0;
}

.mo-md-pic img {
  width: 100%;
  display: block;
}

.mo-md-link {
  font-size: 0.875em;
  margin: 0 0 0 calc(var(--spacer) / 2);
  text-decoration: underline;
  font-weight: normal;
}

/* Cards */
/* Used in: bookPost, recommendationCard, booksReviewCard, userReviewCard */
.book {
  padding: var(--spacer);
  background-color: var(--c-bckgr-primary);
  overflow: hidden;
  width: 100%;
}

.book__cover {
  width: 100%;
  max-width: 150px;
  margin: 0 auto;
}

@media screen and (min-width: 400px) {
  .book__cover {
    float: left;
    margin: 0 var(--spacer) 0 0;
  }
}

.book__cover img {
  width: 100%;
}

.book__link {
  text-decoration: none;
}

.book__title {
  font-size: 1.25em;
  color: var(--c-accent);
  margin: var(--spacer) 0 calc(var(--spacer)/2) 0;
}

@media screen and (min-width: 400px) {
  .book__title {
    margin-top: 0;
  }
}

.book__pages {
  margin-bottom: calc(var(--spacer) / 2);
  font-size: 0.875em;
}

.book__owner {
  margin-bottom: var(--spacer);
  font-size: 0.875em;
}

.book__text {
  font-size: 0.875em;
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
