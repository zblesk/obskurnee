<template>
<section>
  <h1 id="tableLabel">
    {{ poll.title }}<small v-if="poll.isClosed">Uzavreté</small>
  </h1>
  <ol>
    <li v-for="option in poll.options" v-bind:key="option.postId">
      <a @click="toggleShow(option)"><strong>{{ option.bookTitle }}</strong> - {{ option.author }}</a>
    </li>
  </ol>
  
  <book-recommendation v-if="previewId" v-bind:key="previewId.postId" v-bind:post="previewId" ></book-recommendation>
</section>
</template>


<script>
import axios from "axios";
import BookRecommendation from './BookRecommendation.vue';
//import BookRecommendation from "./BookRecommendation.vue";

export default {
  components: { BookRecommendation },
  name: "Poll",
  //components: { BookRecommendation },
  data() {
    return {
      poll: {},
      previewId: null,
    };
  },
  methods: {
    gePollData() {
      axios
        .get("/api/polls/" + this.$route.params.pollId)
        .then((response) => {
          console.log(response.data);
          this.poll = response.data;
        })
        .catch(function (error) {
          alert(error);
        });
    },
    toggleShow(postId){
      if (this.previewId == postId)
      {
        this.previewId = null;
      }
      else 
      {
        this.previewId = postId;
      }
    }
  },
  mounted() {
    this.gePollData();
  },
};
</script>

<style>
</style>