<template>
<div class="page-container">
  <nav-menu></nav-menu>
  <div class="page-center">
    <router-view />
  </div>
  <footer class="footer">
    <p class="attribution">Icons made by <a href="https://www.freepik.com" title="Freepik">Freepik</a> and <a href="https://www.flaticon.com/authors/pixel-perfect" title="Pixel perfect">Pixel perfect</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a>. | Powered by <a href="https://github.com/zblesk/obskurnee" title="Obskurnee">Obskurnee</a> 💚</p>
  </footer>
</div>
</template>

<script>
import { mapActions } from 'vuex'
import NavMenu from './components/NavMenu.vue'

export default {
  name: 'App',
  components: 
  {
    NavMenu
  },
  created () 
  {
    this.reloadSiteData();
    this.restoreContext();
  },
  methods: 
  {
    ...mapActions('context', ['restoreContext']),
    ...mapActions('global', ['reloadSiteData']),
  }
}
</script>

<style>

.todo {
  margin: 2rem;
  color: orangered;
  text-align: center;
}

.todo-l {
  margin: 2rem;
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
  font-size: 2rem;
  font-weight: bold;
  text-align: center;
  margin-top: calc(1.5 * var(--spacer));
  margin-bottom: var(--spacer);
}

.alert-inline {
  color: var(--c-accent);
  font-size: 1.2rem;
  font-weight: bold;
  margin-top: calc(var(--spacer) / 2);
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
  grid-template-columns: 1fr;
  gap: var(--spacer);
  margin: 0;
  padding: var(--spacer);
}

@media screen and (min-width: 450px) {
  .grid {
    grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));
  }
}

/* buttons */
.button-primary,
.button-secondary {
  border-radius: 4px;
  font-size: 1rem;
  font-weight: bold;
  text-decoration: none;
  padding: 0.625rem 1.25rem;
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

.buttons--padding {
  padding: 0 var(--spacer) var(--spacer) var(--spacer);
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
  font-size: 0.875rem;
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
  font-size: 1rem;
  margin-bottom: calc(var(--spacer) / 3);
}

.form-field input {
  font-size: 0.875rem;
  width: 100%;
  border: 0;
  background-color: var(--c-bckgr-secondary);
  padding: calc(var(--spacer) / 2);
}

.form-field textarea {
  font-size: 0.875rem;
  border: 0;
  background-color: var(--c-bckgr-secondary);
  padding: calc(var(--spacer) / 2);
  width: 100%;
  height: 15rem;
}

.form-horizontal {
  display: flex;
  flex-wrap: wrap;
  align-items: baseline;
  margin-bottom: var(--spacer);
}

.form-horizontal label {
  font-size: 1rem;
  margin-right: var(--spacer);
  margin-bottom: calc(var(--spacer) / 3);
}

.form-horizontal select {
  font-size: 0.875rem;
  border: 0;
  background-color: var(--c-bckgr-secondary);
  padding: calc(var(--spacer) / 2);
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
  font-size: 0.875rem;
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

  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.book__cover {
  width: 100%;
  max-width: 150px;
  margin: 0 auto var(--spacer) auto;
}

@media screen and (min-width: 400px) {
  .book__cover {
    float: left;
    max-width: 170px;
    margin: 0;
    padding-right: var(--spacer);
    padding-bottom: var(--spacer);
    background-color: var(--c-bckgr-primary);
  }
}

.book__cover img {
  width: 100%;
  display: block;
}

.book__link {
  text-decoration: none;
}

.book__title {
  font-size: 1.25rem;
  color: var(--c-accent);
  margin: 0 0 calc(var(--spacer)*0.4) 0;
  text-align: center;
  padding-right: 25px;
}

@media screen and (min-width: 400px) {
  .book__title {
    text-align: left;
  }
}

.book__author {
  font-size: 0.875rem;
}

.book__pages {
  margin-bottom: calc(var(--spacer)*0.4);
  font-size: 0.875rem;
}

.book__owner {
  margin-bottom: var(--spacer);
  font-size: 0.875rem;
}

.book__owner a {
  text-decoration: none;
  font-weight: bold;
}

.book__text {
  font-size: 0.875rem;
}

.book__repost {
  text-align: center;
  padding-top: calc(var(--spacer) / 2);
  border-top: 1px solid lightgray;
  margin-top: var(--spacer);
}

.button-repost {
  font-size: 0.875rem;
  text-align: center;
  padding: 5px;
}

.book__top {
  position: relative;
}

.book__permalink {
  position: absolute;
  top: 0px;
  right: 0px;
  text-decoration: none;
  color: var(--c-accent);
}

.book__edit {
  position: absolute;
  top: 24px;
  right: 0px;
  text-decoration: none;
  color: var(--c-accent);
}

/* wrapper for wide book posts */
/* Used in: SinglePost, SingleRecommendation, Book */
.wrapper {
  max-width: 800px;
  margin: calc(var(--spacer) * 2) var(--spacer) var(--spacer) var(--spacer);
  background-color: var(--c-bckgr-primary);
}

@media screen and (min-width: 840px) {
  .wrapper {
    margin: calc(var(--spacer) * 2) auto var(--spacer) auto;
  }
}

/* utility classes */
.u-mt-sp2 {
  margin-top: calc(var(--spacer) * 2);
}

.u-mt-sp {
  margin-top: var(--spacer);
}

.u-mb-sp2 {
  margin-bottom: calc(var(--spacer) * 2);
}

.u-mb-sp05 {
  margin-bottom: calc(var(--spacer) / 2);
}


/* generic styling for markdown */

p {
  font-size: 1rem;
  line-height: 1.5;
  color: var(--c-font);
  margin-bottom: calc(var(--spacer) / 2);
}

p:last-child {
  margin-bottom: 0;
}

.book__pitch p {
  font-size: 0.875rem;
}

.book__pitch h2 {
  font-size: 1rem;
  margin: 0 0 0.5rem 0;
}

h1, h2, h3 {
  color: #595959;
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
  font-size: 1.5rem;
  margin-bottom: var(--spacer);
}

h2 {
  font-size: 1.2rem;
  margin-bottom: calc(var(--spacer) / 2);
}

h3 {
  font-size: 1.15rem;
  margin-bottom: calc(var(--spacer) / 2);
}

blockquote {
  margin-left: var(--spacer);
  font-style: italic;
  margin-bottom: calc(var(--spacer) / 2);
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

.spoiler,
.spoiler p,
.spoiler h3 {
  color: lightgray;
  background-color: lightgray;
}

div.spoiler:not(:last-child) {
  margin-bottom: calc(var(--spacer) / 2);
}

.spoiler:hover, .spoiler:active,
.spoiler p:hover, .spoiler p:active,
.spoiler h3:hover, .spoiler h3:active {
  background-color: var(--c-bckgr-primary);
  color: var(--c-font);
  text-decoration: underline solid lightgray;
}

/* footer */
.footer {
  background-color: var(--c-bckgr-primary);
  padding: calc(var(--spacer) / 2);
}

.attribution {
  font-size: 0.875rem;
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

.searchresult {
    background-color: orange;
} 
</style>
