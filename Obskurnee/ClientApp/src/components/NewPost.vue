<template>
<div>
  <div v-if="hide" class="button-show-wrapper">
    <button @click="toggleVisibility" class="button-primary button-show">Přidat nový návrh</button>
  </div>
  <div v-else class="form">
    <div class="cover">
      <img :src="newpost.imageUrl" v-if="newpost.imageUrl" :alt="newpost.title" />
    </div>
    <div class="form-text">
      <div v-if="mode != 'Themes'" class="form-field">
        <label for="grlink">Odkaz na Goodreads</label>
        <div class="note u-mb-sp05">
          <div class="note-pic">
              <img src="../assets/lamp.svg" alt="lamp icon" />
          </div>
          <p class="note-text">Vlož odkaz, zmáčkni enter nebo tabulátor a chvíli počkej.</p>
        </div>
        <input v-model="newpost.url" placeholder="https://www.goodreads.com/book..." @change="linkChange" id="grlink" />
        <p v-if="fetchInProgress" class="alert-inline">Kamo počkaj, LOADUJEM</p>
      </div>
      <div class="cover-mobile">
        <img :src="newpost.imageUrl" v-if="newpost.imageUrl" :alt="newpost.title" />
      </div>
      <div class="form-field">
        <label for="name" v-if="mode == 'Books' || mode == 'Recommendations'">Název knihy*</label>
        <label for="name" v-if="mode == 'Themes'">Název tématu*</label>
        <input v-model="newpost.title" id="name" required />
      </div>
      <div class="form-field" v-if="mode != 'Themes'">
        <label for="author">Jméno autora*</label>
        <input v-model="newpost.author" id="autor" required />
      </div>
      <div class="form-field" v-if="mode != 'Themes'">
        <label for="pages">Počet stran*</label>
        <input type="number" v-model="newpost.pageCount" id="pages" required />
      </div>
      <div class="form-field u-mb-0">
        
        <div class="label-md-wrapper">
          <label for="text">Komentár k návrhu*</label>
          <div class="mo-md">
            <div class="mo-md-pic">
              <img src="../assets/Markdown-mark.svg" alt="markdown logo">
            </div>
            <div class="mo-md-link">
              <a href="https://www.markdownguide.org/cheat-sheet/">Pomoc s Markdownom</a>
            </div>
          </div>
        </div>
        <textarea v-model="newpost.text" id="text" required placeholder="Markdown umoznuje jednoduche formatovanie textu. Medzi zaklady patri napriklad: 

  # Najvacsi nadpis
  ## mensi nadpis 

  **tucny textt** alebo _kurziva_ 

  - necislovany
  - zoznam
  - je 
  - jednoduchy

  1. cislovany
  2. tiez
  3. lahky

  > takto sa pise citat
  > **moze** obsahovat aj _**formatovanie**_

  Mozes lahko pridat aj [link](https://google.sk)"></textarea>
      </div>
      <p class="asterisk">Pole označená * jsou povinná.</p>
      <div class="buttons">
        <button @click="addPost" class="button-primary">Přidat</button>
        <button @click="toggleVisibility" class="button-secondary hide-form">Schovat formulář</button>
      </div>
    </div>
  </div>
</div>
</template>

<style scoped>

  .button-show {
    display: block;
    width: 100%;
  }

  @media screen and (min-width: 576px) {
    .button-show-wrapper {
      display: flex;
      justify-content: center;
    }

    .button-show {
      display: inline-block;
      width: auto;
    }
  }

  .cover-mobile {
    max-width: 240px;
    margin: 0 auto var(--spacer) auto;
  }

  .cover {
    max-width: 240px;
    display: none;
  }

  .cover img,
  .cover-mobile img {
    width: 100%;
  }

  @media screen and (min-width: 840px) {
    .form {
      display: flex;
      flex-direction: row-reverse;
    }

    .form-text {
      flex-grow: 1;
    }

    .cover-mobile {
      display: none;
    }

    .cover {
      display: block;
      margin: 0 0 0 var(--spacer);
    }
  }

  .asterisk {
    font-size: 0.875em;
    opacity: 0.8;
    margin-top: calc(var(--spacer) /2);
  }

  .form-field.u-mb-0 {
    margin-bottom: 0;
  }

  .hide-form {
    margin-top: var(--spacer);
  }

  @media screen and (min-width: 576px) {
    .hide-form {
      margin-top: 0;
      margin-left: var(--spacer);
    }
  }



</style>

<script>
import axios from "axios";
export default {
  name: "BookPost",
  props: ['mode'],
  emits: ['new-post'],
  data() {
    return {
      newpost: {},
      fetchInProgress: false,
      hide: true,
    };
  },
  methods: {
    addPost() {
      this.$emit('new-post', this.newpost);
      this.newpost = {};
    },
    linkChange() {
      if (!this.newpost.url 
        || !this.newpost.url.startsWith("https://www.goodreads.com"))
      {
        return;
      }
      this.fetchInProgress = true;
      this.$notifyNormal("Nacitavam data");
        axios.get(
          "/api/scrape/?goodreadsUrl=" + this.newpost.url)
        .then((response) => {
          this.newpost = response.data;
          this.newpost.text = "\n\n## Popis na Goodreads: \n\n" + response.data.description;
          this.fetchInProgress = false;
          this.$notifyInfo("Nacitane");
        })
        .catch(function (error) {
          this.$notifyError(error);
          this.fetchInProgress = false;
        });
    },
    toggleVisibility()
    {
      this.hide = !this.hide;
    }
  }
}
</script>
