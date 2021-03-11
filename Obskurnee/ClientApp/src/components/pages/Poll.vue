<template>
<section>
  <h1 id="tableLabel" class="page-title">
    {{ poll.title }}<span class="poll-closed" v-if="poll.isClosed">&nbsp;Uzavreté</span>
  </h1>

  <div v-if="poll.isClosed" class="winner">
    <h2 class="winner-title">Víťaz</h2>
    <book-preview v-if="poll.followupLink?.kind == 'Book'" :book="{ bookId: poll.followupLink.entityId, post: poll.options.find(o => o.postId == poll.results.winnerPostId) }"></book-preview>
    <router-link v-if="poll.followupLink?.kind == 'Discussion'" :to="{ name: 'discussion', params: { discussionId: poll.followupLink.entityId } }" class="link">
      <text-post :post="poll.options.find(o => o.postId == poll.results.winnerPostId)"></text-post>
    </router-link>
  </div>

  <div class="page-wrapper flex">

    <div class="page flex">

      <div v-if="!poll.isClosed && poll.results && poll.results.yetToVote">
        <h2 class="subtitle">Stav hlasování</h2>
        <p class="paragraph">Už hlasovalo {{ poll.results.alreadyVoted }} z {{ poll.results.totalVoters }} čtenářů.</p>
        <p class="paragraph">Ješte nehlasovali: <span v-for="person in poll.results.yetToVote" v-bind:key="person">{{ person }},</span></p>
      </div>

      <div v-if="iVoted || (poll.isClosed && poll.results?.votes)">
        <h2 class="subtitle">Výsledky</h2>
        <ol class="poll">
          <li class="poll-field" v-for="vote in poll.results.votes" v-bind:key="vote">
            {{ poll.options.find(o => o.postId == vote.postId).title }}:
             {{ vote.votes }} hlasy - {{ vote.percentage }}%
          </li>
        </ol>
      </div>

      <h2 v-if="poll.results" class="subtitle u-mt">Možnosti</h2>
      <ol class="poll">
        <li v-for="option in poll.options" v-bind:key="option.postId" class="poll-field">
          <input type="checkbox" :id="option.postId" :value="option.postId" v-model="checkedOptions" :disabled="iVoted || poll.isClosed" class="checkbox" />
          <label :for="option.postId" @click="toggleShow(option)"><span class="label-title">{{ option.title }}</span> <span v-if="option.author">({{ option.author }})</span></label>
        </li>
      </ol>

      <div class="buttons">
        <button @click="vote" :disabled="!checkedOptions?.length" class="button-primary" v-if="!iVoted && !poll.isClosed">Hlasovat</button>
        <button v-if="isMod && !poll.isClosed" @click="closePoll(poll.pollId)" class="button-secondary poll-to-close">Zavri hlasovanie hned</button>
      </div>
      
    </div>

    <div class="book-post-wrapper flex">
      <book-post v-if="previewId" v-bind:key="previewId.postId" v-bind:post="previewId" ></book-post>
    </div>

  </div>


</section>
</template>


<script>
import BookPost from '../BookPost.vue';
import BookPreview from '../BookPreview.vue';
import TextPost from '../TextPost.vue';
import { mapGetters,mapActions, mapState } from "vuex";

export default {
  components: { BookPost, TextPost, BookPreview },
  name: "Poll",
  data() {
    return {
      previewId: null,
      checkedOptions: [],
      iVoted: false,
    };
  },
  computed: {
    ...mapGetters("context", ["isMod"]),
    ...mapState("polls", ["polls", "votes"]),
    poll: function () { return this.polls[this.$route.params.pollId] ?? { options: [] }; },
  },
  methods: {
    ...mapActions("polls", ["getPollData", "sendVote", "closePoll"]),
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
      if (!this.checkedOptions?.length)
      {
        return;
      }
      this.sendVote( { pollId: this.$route.params.pollId, votes: this.checkedOptions })
        .then((response) => {
          console.log('klientsky respons', response);
          this.iVoted = true;
        })
        .catch(function (error) {
          this.$notifyError(error);
        });
    },
  },
  mounted() {
    this.getPollData(this.$route.params.pollId)
      .then(() => {
        this.checkedOptions = this.votes[this.$route.params.pollId] ?? [];
        if (this.checkedOptions?.length)
        {
          this.iVoted = true;
        }
      });
  },
};
</script>

<style scoped>

  .poll-closed {
    font-size: 0.65em;
    color: var(--c-accent);
    text-transform: uppercase;
    vertical-align: super;
  }

  .buttons {
    display: flex;
    flex-direction: column;
  }

  .poll-to-close {
    margin-top: var(--spacer);
  }

  @media screen and (min-width: 576px) {
    .buttons {
      flex-direction: row;
    }

    .poll-to-close {
      margin-top: 0;
      margin-left: var(--spacer);
    }
  }

  .subtitle {
    font-size: 1.25em;
    margin-top: 0;
    margin-bottom: calc(var(--spacer) / 2);
  }
  
  .u-mt {
    margin-top: calc(var(--spacer) * 2);
  }

  .paragraph {
    margin-bottom: calc(var(--spacer) / 2);
  }

  .link {
    text-decoration: none;
  }

  .winner-title {
    text-align: center;
    font-size: 1.25em;
    margin-top: 0;
    margin-bottom: 0;
  }

  /* layout */

  .page,
  .winner {
    max-width: 800px;
    background-color: var(--c-bckgr-primary);
    margin: var(--spacer);
    padding: calc(2* var(--spacer));
  }

  @media screen and (min-width: 840px) {
    .page,
    .winner {
      margin: var(--spacer) auto;
    }
  }

  .book-post-wrapper {
    max-width: 800px;
    margin: var(--spacer);
  }

  @media screen and (min-width: 840px) {
    .book-post-wrapper {
      margin: var(--spacer) auto;
    }
  }

  @media screen and (min-width: 1200px) {
    .page-wrapper.flex {
      display: flex;
      max-width: 1660px;
      margin: 0 auto;
    }

    .book-post-wrapper.flex {
      flex: 1 1 50%;
      margin-left: var(--spacer);
      margin-right: var(--spacer);

      display: flex;
    }

    .page.flex {
      flex: 1 1 50%;
      margin-left: var(--spacer);
    }
  }

  /* open poll */

  .poll {
    margin: 0 0 var(--spacer) 0;
    padding-left: calc(var(--spacer) * 2);
  }

  .poll-field:not(:last-child) {
    margin-bottom: calc(var(--spacer) / 2);
  }

  .poll-field input {
    margin-right: calc(var(--spacer) / 2);
    margin-left: calc(var(--spacer) / 2);
  }

  .checkbox:checked+label>.label-title {
    font-weight: bold;
  }

  
  



</style>