<template>
<div class="form">
  <div class="cover">
    <img :src="newpost.imageUrl" v-if="newpost.imageUrl" />
  </div>
  <div class="form-text">
    <div v-if="mode == 'Books'" class="form-field">
      <label for="grlink">Odkaz na Goodreads</label>
      <div class="note">
        <div class="note-pic">
            <img src="../assets/lamp.svg" alt="lamp icon" />
        </div>
        <p class="note-text">Vlož odkaz, zmáčkni enter nebo tabulátor a chvíli počkej.</p>
      </div>
      <input v-model="newpost.url" placeholder="https://www.goodreads.com/book..." @change="linkChange" id="grlink" />
      <div v-if="fetchInProgress" class="alert">Kamo počkaj, LOADUJEM</div>
    </div>
    <div class="form-field">
      <label for="name">Název knihy nebo tématu</label>
      <input v-model="newpost.title" id="name" required />
    </div>
    <div class="form-field" v-if="mode == 'Books'">
      <label for="author">Jméno autora</label>
      <input v-model="newpost.author" id="autor" />
    </div>
    <div class="form-field" v-if="mode == 'Books'">
      <label for="pages">Počet stran</label>
      <input v-model="newpost.pageCount" id="pages" />
    </div>
    <div class="form-field">
      <label for="text">Komentár k návrhu (podporuje markdown)</label>
      <textarea v-model="newpost.text" id="text" required></textarea>
    </div>
    <button @click="postNewBook" class="button-primary">Pridaj</button>
  </div>
</div>
</template>

<style scoped>

  .cover {
    max-width: 250px;
    margin: 0 auto var(--spacer) auto;
  }

  .cover img {
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

    .cover {
      margin: 0 0 0 var(--spacer);
    }
  }

  .alert {
    color: var(--c-accent);
    font-size: 1.2em;
    font-weight: bold;
    margin-top: calc(var(--spacer) / 2);
  }

  .note {
    display: flex;
    align-items: center;
    margin-bottom: calc(var(--spacer) / 2);
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

  .form-field {
    margin-bottom: var(--spacer);
  }

  .form-field label {
    display: block;
    font-size: 1em;
    margin-bottom: calc(var(--spacer) / 3);
  }

  .form-field input {
    width: 100%;
    border: 0;
    background-color: var(--c-bckgr-secondary);
    padding: 0.5em;
  }

  .form-field textarea {
    font-size: 1em;
    border: 0;
    background-color: var(--c-bckgr-secondary);
    padding: 0.5em 1em;
    width: 100%;
    height: 15em;
  }



</style>

<script>
import axios from "axios";
import { mapActions } from "vuex";
export default {
  name: "BookPost",
  props: ['mode', "discussionId"],
  data() {
    return {
      newpost: {},
      fetchInProgress: false,
    };
  },
  methods: {
    ...mapActions("discussions", ["newPost"]),
    postNewBook() {
        this.newPost({ discussionId: this.discussionId, newPost: this.newpost })
          .then(() => this.newpost = {} );
    },
    linkChange() {
      if (!this.newpost.url 
        || !this.newpost.url.startsWith("https://www.goodreads.com"))
      {
        console.log("Not fetching ", this.newpost.url);
        return;
      }
      this.fetchInProgress = true;
        axios.get(
          "/api/scrape/?goodreadsUrl=" + this.newpost.url)
        .then((response) => {
          this.newpost = response.data;
          this.newpost.text = "\n\nPopis na Goodreads: \n\n" + response.data.description;
          this.fetchInProgress = false;
        })
        .catch(function (error) {
          alert(error);
          this.fetchInProgress = false;
        });
    },
  }
}
</script>
