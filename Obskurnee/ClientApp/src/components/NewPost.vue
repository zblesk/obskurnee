<template>
<div>
  <span v-if="mode == 'Books'">
    <input v-model="newpost.url" placeholder="https://www.goodreads.com/book..." @change="linkChange"/>
    <span style="color: cyan" v-if="fetchInProgress">Kamo pockaj, LOADUJEM</span>
  </span>
  <input v-model="newpost.title" placeholder="Meno knihy" required />
  <input v-if="mode == 'Books'" v-model="newpost.author" placeholder="Autor" />
  <input v-if="mode == 'Books'" v-model="newpost.pageCount" placeholder="Počet strán" />
  <textarea v-model="newpost.text" placeholder="Komentár k návrhu" required></textarea>
  <button @click="postNewBook">Pridaj</button>
  <img :src="newpost.imageUrl" v-if="newpost.imageUrl" />
</div>
</template>


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
