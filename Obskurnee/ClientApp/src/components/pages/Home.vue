<template>
<section>
  <span class="todo-l" v-if="currentPoll || currentDiscussion">
    Vitaj, {{ myProfile.name }}. 
    <span v-if="currentPoll">Práve beží <router-link :to="{ name: 'poll', params: { pollId: currentPoll.pollId } }">hlasovanie.</router-link></span>
    <span v-if="currentDiscussion">Zbierame <router-link :to="{ name: 'discussion', params: { discussionId: currentDiscussion.discussionId } }">návrhy!</router-link></span>
  </span>
  <div class="grid2">
  <book-large-card :post="currentBook.post" style="margin: auto;" v-if="currentBook && currentBook.post">
      <h5><em>Momentálne čítame knihu #{{ currentBook.order }}</em></h5>
  </book-large-card>
  <div v-if="isAuthenticated && noticeboardHtml" style="border-color: navy; border-style: dotted dashed solid double;" v-html="noticeboardHtml"></div>
  <div v-if="isAuthenticated && userProfileIncomplete" style="border-color: navy; border-style: dotted dashed solid double;">
    Este nemas vyplneny profil. 
    <router-link :to="{ name: 'user', params: { email: myProfile.email, mode: 'edit' } }">Prosím doplň ho.</router-link> <br />
    Zatiaľ máme: <br />
    <p v-if="myProfile.name">{{ myProfile.name }}</p>
    <p v-if="myProfile.phone">{{ myProfile.phone }}</p>
    <p v-if="myProfile.aboutMe" :v-html="myProfile.aboutMeHtml"></p>
    <p v-if="myProfile.goodreadsUrl">{{ myProfile.goodreadsUrl }}</p>
  </div>
  </div>
  
  <div class="grid" v-if="books && isAuthenticated">
    <book-preview v-for="book in books" v-bind:key="book.bookId" v-bind:book="book">Kniha #{{book.order}}</book-preview>
  </div>
</section>
</template>

<script>
import BookPreview from '../BookPreview.vue';
import axios from 'axios';
import BookLargeCard from '../BookLargeCard.vue';
import { mapGetters } from "vuex";

export default {
  components: { BookPreview, BookLargeCard },
  name: 'Home',
  data() {
      return {
        books: [],
        currentBook: {},
        myProfile: {},
        noticeboardHtml: "",
        currentPoll: null,
        currentDiscussion: null,
      }
  },
  methods: {
    getBooks() {
        axios.get('/api/home')
            .then((response) => {
              if (response.data.books.length)
              {
                this.books = response.data.books;
                this.currentBook = this.books.shift();
              }
              this.noticeboardHtml = response.data.notice;
              this.myProfile = response.data.myProfile;
              this.currentPoll = response.data.currentPoll;
              this.currentDiscussion = response.data.currentDiscussion ;
            })
            .catch(function (error) {
                console.log(error);
            });
    },
  },
  computed: {
      ...mapGetters("context", ["isAuthenticated"]),
      userProfileIncomplete: function() { return this.myProfile && !(this.myProfile.name && this.myProfile.phone && this.myProfile.aboutMe && this.myProfile.goodreadsUrl); },
  },
  mounted() {
      this.getBooks();
  }
}
</script>

<style scoped>
.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(100px, 220px));
  gap: 10px;
  font-family: "Raleway", sans-serif;
  padding: 4em;
}
.grid2 {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(100px, 500px));
  gap: 10px;
  font-family: "Raleway", sans-serif;
  padding: 4em;
}
</style>