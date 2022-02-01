<template>
<div class="form-field u-mb-0">
    <div class="label-md-wrapper">
    <label for="text">{{$t('newpost.comment')}}<span v-if="required">*</span></label>
    <div class="mo-md">
        <div class="mo-md-pic">
        <img src="../assets/Markdown-mark.svg" alt="null">
        </div>
        <div class="mo-md-link">
        <markdown-help-link tabindex="7"></markdown-help-link>
        </div>
    </div>
    </div>
    <textarea :value="modelValue"
        @input="$emit('update:modelValue', $event.target.value)"
        id="text" 
        :required="required" 
        :placeholder="$t('global.markdownSamplePlaceholder')" 
        tabindex="5"></textarea>
</div>
</template>

<script>
import MarkdownHelpLink from "./MarkdownHelpLink.vue";
export default {
  name: "MarkdownEditor",
  components: { MarkdownHelpLink },
  props: {
    modelValue: {
      type: String,
      required: false,
    },
    required: {
        type: Boolean,
        required: false,
        default: false
    },
  },
  emits: ['update:modelValue'],
}
</script>
