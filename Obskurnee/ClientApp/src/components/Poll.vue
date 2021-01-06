<template>
  <h1 id="tableLabel">
    {{ poll.title }}<small v-if="poll.isClosed">Uzavreté</small>
  </h1>
  <ol>
    <li v-for="option in poll.options" v-bind:key="option.pollOptionId">
      {{ option.title }}
    </li>
  </ol>
</template>


<script>
import axios from "axios";
//import BookRecommendation from "./BookRecommendation.vue";

export default {
  name: "Poll",
  //components: { BookRecommendation },
  data() {
    return {
      poll: {},
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
  },
  mounted() {
    this.gePollData();
  },
};
</script>

<style>
</style>