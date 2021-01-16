<template>
<section>
  <h1 id="tableLabel">
    {{ poll.title }}<small v-if="poll.isClosed"> Uzavreté</small>
  </h1>

  <div v-if="poll.isClosed && poll.bookId">
    
  </div>

  <div v-if="!poll.isClosed && pollResults && pollResults.yetToVote" 
    style="margin:1em; border-color: pink; border-style:dashed;">
    Už hlasovalo {{ pollResults.alreadyVoted }} z {{ pollResults.totalVoters }}. <br />
    Ešte nehlasovali: <span v-for="person in pollResults.yetToVote" v-bind:key="person">{{ person }}</span>
  </div>
  <ol>
    <li v-for="option in poll.options" v-bind:key="option.postId">
      <input type="checkbox" :id="option.postId" :value="option.postId" v-model="checkedOptions" :disabled="iVoted"/>
      <label :for="option.postId" @click="toggleShow(option)"><strong>{{ option.bookTitle }}</strong> - {{ option.author }}</label>
    </li>
  </ol>
  <button @click="vote" :disabled="!checkedOptions.length" class="btn btn-warning">Hlasuj!</button>
  
  <div v-if="iVoted && pollResults && pollResults.votes">
    <h2>VÝSLEDKY:</h2>
    <ol>
      <li v-for="vote in pollResults.votes" v-bind:key="vote">
         {{ poll.options.find(o => o.postId == vote.postId).bookTitle }} 
         - {{ vote.votes }} hlasy - {{ vote.percentage }}%
      </li>
    </ol>
  </div>

  <book-recommendation v-if="previewId" v-bind:key="previewId.postId" v-bind:post="previewId" ></book-recommendation>
</section>
</template>


<script>
import axios from "axios";
import BookRecommendation from './BookRecommendation.vue';

export default {
  components: { BookRecommendation },
  name: "Poll",
  data() {
    return {
      poll: {},
      previewId: null,
      checkedOptions: [],
      iVoted: false,
      pollResults: {}
    };
  },
  methods: {
    gePollData() {
      axios
        .get("/api/polls/" + this.$route.params.pollId)
        .then((response) => {
          console.log(response.data);
          this.poll = response.data.poll;
          this.pollResults = response.data.poll.results;
          if (response.data.myVote)
          {
             this.checkedOptions = response.data.myVote.postIds;
             this.iVoted = this.checkedOptions.length;
          }
          
      // for (let option in this.poll.options)
      // {
      //   console.log(option);
      //   let opResult = this.pollResults.votes[option.postId];
      //   console.log(opResult);
        
      // }
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
      axios.post(
          "/api/polls/" + this.$route.params.pollId + "/vote",
          { postIds: this.checkedOptions })
        .then((response) => {
          this.iVoted = true;
          this.pollResults = response.data;
          console.log(response.data);
        })
        .catch(function (error) {
          alert(error);
        });
    },
    
    orderedResults() {
      let lst = [];
      return lst;
    }
  },
  computed: {
  },
  mounted() {
    this.gePollData();
  },
};
</script>

<style>
</style>