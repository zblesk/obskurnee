<template>
<section>
  <h1 id="tableLabel">
    {{ poll.title }}<small v-if="poll.isClosed">Uzavreté</small>
  </h1>
  <ol>
    <li v-for="option in poll.options" v-bind:key="option.postId">
      <input type="checkbox" :id="option.postId" :value="option.postId" v-model="checkedOptions" :disabled="iVoted"/>
      <label :for="option.postId" @click="toggleShow(option)"><strong>{{ option.bookTitle }}</strong> - {{ option.author }}</label>
    </li>
  </ol>
  <button @click="vote" v-if="!iVoted" :disabled="!checkedOptions.length" class="btn btn-warning">Hlasuj!</button>
  
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
      checkedOptions: [],
      iVoted: false
    };
  },
  methods: {
    gePollData() {
      axios
        .get("/api/polls/" + this.$route.params.pollId)
        .then((response) => {
          console.log(response.data);
          this.poll = response.data.poll;
          if (response.data.myVote)
          {
             this.checkedOptions = response.data.myVote.postIds;
             this.iVoted = this.checkedOptions.length;
          }
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
    },
    vote()
    {
      if (!this.checkedOptions.length)
      {
        console.log("You must select something first");
        return;
      }
      console.log(this.checkedOptions);
      
      axios.post(
          "/api/polls/" + this.$route.params.pollId + "/vote",
          { postIds: this.checkedOptions })
        .then((response) => {
          this.iVoted = true;
          console.log(response);
        })
        .catch(function (error) {
          alert(error);
        });
    }
  },
  mounted() {
    this.gePollData();
  },
};
</script>

<style>
</style>