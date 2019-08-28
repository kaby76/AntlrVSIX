// Derived from https://github.com/kaby76/rust-grammar
// and from https://github.com/rust-lang/rust/blob/master/src/grammar


parser grammar RustParser;

options {
	tokenVocab = RustLexer;
}


// === Modules and items

crate:
    maybe_shebang mod_body EOF;

maybe_shebang
	: SHEBANG_LINE
	|
	;

mod_body:
    inner_attr* item*;

visibility:
    PUB visibility_restriction?;

// Note that `pub(` does not necessarily signal the beginning of a visibility
// restriction! For example:
//
//     struct T(i32, i32, pub(i32));
//
// Here the `(` is part of the type `(i32)`.
visibility_restriction:
    OP CRATE CP
    | OP SUPER CP
    | OP IN ident CP;

item:
    attr* visibility? pub_item
    | attr* impl_block
    | attr* extern_mod
    | attr* item_macro_use;

pub_item:
    extern_crate     // `pub extern crate` is deprecated but still allowed
    | use_decl
    | mod_decl_short
    | mod_decl
    | static_decl
    | const_decl
    | fn_decl
    | type_decl
    | struct_decl
    | enum_decl
    | union_decl
    | trait_decl;

item_macro_use:
    item_macro_path NOT ident? item_macro_tail;

item_macro_path:
    ident
    | item_macro_path_parent? COCO ident;  // experimental `feature(use_extern_macros)`

item_macro_path_parent:
    SELF
    | item_macro_path_segment
    | COCO item_macro_path_segment
    | item_macro_path_parent COCO item_macro_path_segment;

item_macro_path_segment:
    ident
    | SUPER;

item_macro_tail:
    tt_parens SEMI
    | tt_brackets SEMI
    | tt_block;


// --- extern crate

extern_crate:
    EXTERN CRATE ident rename? SEMI;


// --- use declarations

use_decl:
    USE use_path SEMI;

use_path:
    COCO? OC use_item_list CC
    | COCO? any_ident (COCO any_ident)* use_suffix?;

use_suffix:
    COCO STAR
    | COCO OC use_item_list? CC
    | rename;

use_item:
    any_ident rename?;

use_item_list:
    use_item (CO use_item)* CO?;

rename:
    AS ident;


// --- Modules

mod_decl_short:
    MOD ident SEMI;

mod_decl:
    MOD ident OC mod_body CC;


// --- Foreign modules

extern_mod:
    extern_abi OC inner_attr* foreign_item* CC;

foreign_item:
    attr* visibility? foreign_item_tail
    | attr* item_macro_use;

foreign_item_tail:
    STATIC MUT? ident COL ty_sum SEMI
    | TYPE ident SEMI
    | foreign_fn_decl;


// --- static and const declarations

static_decl:
    STATIC MUT? ident COL ty_sum EQ expr SEMI;

const_decl:
    CONST ident COL ty_sum EQ expr SEMI;


// --- Functions

fn_decl:
    fn_head OP param_list? CP fn_rtype? where_clause? block_with_inner_attrs;

method_decl:
    fn_head OP method_param_list? CP fn_rtype? where_clause? block_with_inner_attrs;

trait_method_decl:
    fn_head OP trait_method_param_list? CP rtype? where_clause? (block_with_inner_attrs | SEMI);

foreign_fn_decl:
    fn_head OP variadic_param_list? CP rtype? where_clause? SEMI;

// Parts of a `fn` definition up to the type parameters.
//
// `const` and `extern` are incompatible on a `fn`, but this grammar
// does not rule it out, holding that in a hypothetical Rust language
// specification, it would be clearer to specify this as a semantic
// rule, not a syntactic one. That is, not every rule that can be
// enforced gramatically should be.
fn_head:
    CONST? UNSAFE? extern_abi? FN ident ty_params?;

param:
    pat COL param_ty;

param_ty:
    ty_sum
    | IMPL bound;  // experimental: feature(universal_impl_trait)

param_list:
    param (CO param)* CO?;

variadic_param_list:
    param (CO param)* (CO DOTDOTDOT)? CO?;

variadic_param_list_names_optional:
    trait_method_param (CO trait_method_param)* (CO DOTDOTDOT)? CO?;

self_param:
    MUT? SELF (COL ty_sum)?
    | AND Lifetime? MUT? SELF;

method_param_list:
    (param | self_param) (CO param)* CO?;

// Argument names are optional in traits. The ideal grammar here would be
// `(pat ':')? ty_sum`, but parsing this would be unreasonably complicated.
// Instead, the `pat` is restricted to a few short, simple cases.
trait_method_param:
    restricted_pat COL ty_sum
    | ty_sum;

restricted_pat:
    (AND | ANDAND | MUT)? (UNDERSCORE | ident);

trait_method_param_list:
    (trait_method_param | self_param) (CO trait_method_param)* CO?;

// `ty_sum` is permitted in parameter types (although as a matter of semantics
// an actual sum is always rejected later, as having no statically known size),
// but only `ty` in return types. This means that in the where-clause
// `where T: Fn() -> X + Clone`, we're saying that T implements both
// `Fn() -> X` and `Clone`, not that its return type is `X + Clone`.
rtype:
    RARROW (ty | NOT);

// Experimental `feature(conservative_impl_trait)`.
fn_rtype:
    RARROW (ty | NOT | IMPL bound);


// --- type, struct, and enum declarations

type_decl:
    TYPE ident ty_params? where_clause? EQ ty_sum SEMI;

struct_decl:
    STRUCT ident ty_params? struct_tail;

struct_tail:
    where_clause? SEMI
    | OP tuple_struct_field_list? CP where_clause? SEMI
    | where_clause? OC field_decl_list? CC;

tuple_struct_field:
    attr* visibility? ty_sum;

tuple_struct_field_list:
    tuple_struct_field (CO tuple_struct_field)* CO?;

field_decl:
    attr* visibility? ident COL ty_sum;

field_decl_list:
    field_decl (CO field_decl)* CO?;

enum_decl:
    ENUM ident ty_params? where_clause? OC enum_variant_list? CC;

enum_variant:
    attr* enum_variant_main;

enum_variant_list:
    enum_variant (CO enum_variant)* CO?;

enum_variant_main:
    ident OP enum_tuple_field_list? CP
    | ident OC enum_field_decl_list? CC
    | ident EQ expr
    | ident;

// enum variants that are tuple-struct-like can't have `pub` on individual fields.
enum_tuple_field:
    attr* ty_sum;

enum_tuple_field_list:
    enum_tuple_field (CO enum_tuple_field)* CO?;

// enum variants that are struct-like can't have `pub` on individual fields.
enum_field_decl:
    ident COL ty_sum;

enum_field_decl_list:
    enum_field_decl (CO enum_field_decl)* CO?;

union_decl:
    UNION ident OC field_decl_list CC;


// --- Traits

// The `auto trait` syntax is an experimental feature, `optin_builtin_traits`,
// also known as OIBIT.
trait_decl:
    UNSAFE? AUTO? TRAIT ident ty_params? colon_bound? where_clause? OC trait_item* CC;

trait_item:
    attr* TYPE ident colon_bound? ty_default? SEMI
    | attr* CONST ident COL ty_sum const_default? SEMI  // experimental associated constants
    | attr* trait_method_decl
    | attr* item_macro_path NOT item_macro_tail;

ty_default:
    EQ ty_sum;

const_default:
    EQ expr;


// --- impl blocks

impl_block:
    UNSAFE? IMPL ty_params? impl_what where_clause? OC impl_item* CC;

impl_what:
    NOT ty_sum FOR ty_sum
    | ty_sum FOR ty_sum
    | ty_sum FOR DOTDOT
    | ty_sum;

impl_item:
    attr* visibility? impl_item_tail;

impl_item_tail:
    DEFAULT? method_decl
    | TYPE ident EQ ty_sum SEMI
    | const_decl  // experimental associated constants
    | item_macro_path NOT item_macro_tail;


// === Attributes and token trees

attr:
    POUND OB tt* CB;

inner_attr:
    POUND NOT OB tt* CB;

tt:
    ~(OP | CP | OC | CC | OB | CB)
    | tt_delimited;

tt_delimited:
    tt_parens
    | tt_brackets
    | tt_block;

tt_parens:
    OP tt* CP;

tt_brackets:
    OB tt* CB;

tt_block:
    OC tt* CC;

macro_tail:
    NOT tt_delimited;


// === Paths
// (forward references: ty_sum, ty_args)

// This is very slightly different from the syntax read by rustc:
// whitespace is permitted after `self` and `super` in paths.
//
// In rustc, `self::x` is an acceptable path, but `self :: x` is not,
// because `self` is a strict keyword except when followed immediately
// by the exact characters `::`. Same goes for `super`. Pretty weird.
//
// So instead, this grammar accepts that `self` is a keyword, and
// permits it specially at the very front of a path. Whitespace is
// ignored. `super` is OK anywhere except at the end.
//
// Separately and more tentatively: in rustc, qualified paths are
// permitted in peculiarly constrained contexts. In this grammar,
// qualified paths are just part of the syntax of paths (for now -
// this is not clearly an OK change).

path:
    path_segment_no_super
    | path_parent? COCO path_segment_no_super;

path_parent:
    SELF
    | LT ty_sum as_trait? GT
    | path_segment
    | COCO path_segment
    | path_parent COCO path_segment;

as_trait:
    AS ty_sum;

path_segment:
    path_segment_no_super
    | SUPER;

path_segment_no_super:
    simple_path_segment (COCO ty_args)?;

simple_path_segment:
    ident
    | SELF;


// === Type paths
// (forward references: rtype, ty_sum, ty_args)

ty_path:
    for_lifetime? ty_path_main;

for_lifetime:
    FOR LT lifetime_def_list? GT;

lifetime_def_list:
    lifetime_def (CO lifetime_def)* CO?;

lifetime_def:
    Lifetime (COL lifetime_bound)?;

lifetime_bound:
    Lifetime
    | lifetime_bound PLUS Lifetime;

ty_path_main:
    ty_path_tail
    | ty_path_parent? COCO ty_path_tail;

ty_path_tail:
    (ident | SELF) OP ty_sum_list? CP rtype?
    | ty_path_segment_no_super;

ty_path_parent:
    SELF
    | LT ty_sum as_trait? GT
    | ty_path_segment
    | COCO ty_path_segment
    | ty_path_parent COCO ty_path_segment;

ty_path_segment:
    ty_path_segment_no_super
    | SUPER;

ty_path_segment_no_super:
    (ident | SELF) ty_args?;


// === Type bounds

where_clause:
    WHERE where_bound_list;

where_bound_list:
    where_bound (CO where_bound)* CO?;

where_bound:
    Lifetime COL lifetime_bound
    | for_lifetime? ty colon_bound;

colon_bound:
    COL bound;

bound:
    prim_bound
    | bound PLUS prim_bound;

prim_bound:
    ty_path
    | QU ty_path
    | Lifetime;


// === Types and type parameters

ty:
    UNDERSCORE
    // The next 3 productions match exactly `'(' ty_sum_list? ')'`,
    // but (i32) and (i32,) are distinct types, so parse them with different rules.
    | OP CP                           // unit
    | OP ty_sum CP                    // grouping (parens are ignored)
    | OP ty_sum CO ty_sum_list? CP   // tuple
    | OB ty_sum (SEMI expr)? CB
    | AND Lifetime? MUT? ty
    | ANDAND Lifetime? MUT? ty          // meaning `& & ty`
    | STAR mut_or_const ty               // pointer type
    | for_lifetime? UNSAFE? extern_abi? FN OP variadic_param_list_names_optional? CP rtype?
    | ty_path macro_tail?;

mut_or_const:
    MUT
    | CONST;

extern_abi:
    EXTERN StringLit?;

ty_args:
    LT lifetime_list GT
    | LT (Lifetime CO)* ty_arg_list GT;

lifetime_list:
    Lifetime (CO Lifetime)* CO?;

ty_sum:
    ty (PLUS bound)?;

ty_sum_list:
    ty_sum (CO ty_sum)* CO?;

ty_arg:
    ident EQ ty_sum
    | ty_sum;

ty_arg_list:
    ty_arg (CO ty_arg)* CO?;

ty_params:
    LT lifetime_param_list GT
    | LT (lifetime_param CO)* ty_param_list GT;

lifetime_param:
    attr* Lifetime (COL lifetime_bound)?;

lifetime_param_list:
    lifetime_param (CO lifetime_param)* CO?;

ty_param:
    attr* ident colon_bound? ty_default?;

ty_param_list:
    ty_param (CO ty_param)* CO?;


// === Patterns

pat:
    pat_no_mut
    | MUT ident (AT pat)?;

// A `pat_no_mut` is a pattern that does not start with `mut`.
// It is distinct from `pat` to rule out ambiguity in parsing the
// pattern `&mut x`, which must parse like `&mut (x)`, not `&(mut x)`.
pat_no_mut:
    UNDERSCORE
    | pat_lit
    | pat_range_end DOTDOTDOT pat_range_end
    | pat_range_end DOTDOT pat_range_end  // experimental `feature(exclusive_range_pattern)`
    | path macro_tail
    | REF? ident (AT pat)?
    | REF MUT ident (AT pat)?
    | path OP pat_list_with_dots? CP
    | path OC pat_fields? CC
    | path  // BUG: ambiguity with bare ident case (above)
    | OP pat_list_with_dots? CP
    | OB pat_elt_list? CB  // experimental slice patterns
    | AND pat_no_mut
    | AND MUT pat
    | ANDAND pat_no_mut   // `&& pat` means the same as `& & pat`
    | ANDAND MUT pat
    | BOX pat;

pat_range_end:
    path
    | pat_lit;

pat_lit:
    UNDERSCORE? lit;

pat_list:
    pat (CO pat)* CO?;

pat_list_with_dots:
    pat_list_dots_tail
    | pat (CO pat)* (CO pat_list_dots_tail?)?;

pat_list_dots_tail:
    DOTDOT (CO pat_list)?;

// rustc does not accept `[1, 2, tail..,]` as a pattern, because of the
// trailing comma, but I don't see how this is justifiable.  The rest of the
// language is *extremely* consistent in this regard, so I allow the trailing
// comma here.
//
// This grammar does not enforce the rule that a given slice pattern must have
// at most one `..`.

pat_elt:
    pat DOTDOT?
    | DOTDOT;

pat_elt_list:
    pat_elt (CO pat_elt)* CO?;

pat_fields:
    DOTDOT
    | pat_field (CO pat_field)* (CO DOTDOT | CO?);

pat_field:
    BOX? REF? MUT? ident
    | ident COL pat;


// === Expressions

expr:
    assign_expr;

expr_no_struct:
    assign_expr_no_struct;

expr_list:
    expr (CO expr)* CO?;


// --- Blocks

// OK, this is super tricky. There is an ambiguity in the grammar for blocks,
// `{ stmt* expr? }`, since there are strings that match both `{ stmt expr }`
// and `{ expr }`.
//
// The rule in Rust is that the `{ stmt expr }` parse is preferred: the body
// of the block `{ loop { break } - 1 }` is a `loop` statement followed by
// the expression `-1`, not a single subtraction-expression.
//
// Agreeably, the rule to resolve such ambiguities in ANTLR4, as in JS regexps,
// is the same. Earlier alternatives that match are preferred over later
// alternatives that match longer sequences of source tokens.
block:
    OC stmt* expr? CC;

// Shared by blocky_expr and fn_body; in the latter case, any inner attributes
// apply to the whole fn.
block_with_inner_attrs:
    OC inner_attr* stmt* expr? CC;

stmt:
    SEMI
    | item  // Statement macros are included here.
    | stmt_tail;

// Attributes are supported on most statements.  Let statements can have
// attributes; block statements can have outer or inner attributes, like this:
//
//     fn f() {
//         #[cfg(test)]
//         {
//             #![allow()]
//             println!("testing...");
//         }
//     }
//
// Attributes on block expressions that appear anywhere else are an
// experimental feature, `feature(stmt_expr_attributes)`. We support both.
stmt_tail:
    attr* LET pat (COL ty)? (EQ expr)? SEMI
    | attr* blocky_expr
    | expr SEMI;

// Inner attributes in `match`, `while`, `for`, `loop`, and `unsafe` blocks are
// experimental, `feature(stmt_expr_attributes)`.
blocky_expr:
    block_with_inner_attrs
    | IF cond_or_pat block (ELSE IF cond_or_pat block)* (ELSE block)?
    | MATCH expr_no_struct OC expr_inner_attrs? match_arms? CC
    | loop_label? WHILE cond_or_pat block_with_inner_attrs
    | loop_label? FOR pat IN expr_no_struct block_with_inner_attrs
    | loop_label? LOOP block_with_inner_attrs
    | UNSAFE block_with_inner_attrs;

cond_or_pat:
    expr_no_struct
    | LET pat EQ expr;

loop_label:
    Lifetime COL;

match_arms:
    match_arm_intro blocky_expr CO? match_arms?
    | match_arm_intro expr (CO match_arms?)?;

match_arm_intro:
    attr* match_pat match_if_clause? FAT_ARROW;

match_pat:
    pat
    | match_pat OR pat;

match_if_clause:
    IF expr;


// --- Primary expressions

// Attributes on expressions are experimental.
// Enable with `feature(stmt_expr_attributes)`.
expr_attrs:
    attr attr*;

// Inner attributes in array and struct expressions are experimental.
// Enable with `feature(stmt_expr_attributes)`.
expr_inner_attrs:
    inner_attr inner_attr*;

prim_expr:
    prim_expr_no_struct
    | path OC expr_inner_attrs? fields? CC;

prim_expr_no_struct:
    lit
    | SELF
    | path macro_tail?
    // The next 3 productions match exactly `'(' expr_list ')'`,
    // but (e) and (e,) are distinct expressions, so match them separately
    | OP expr_inner_attrs? CP
    | OP expr_inner_attrs? expr CP
    | OP expr_inner_attrs? expr CO expr_list? CP
    | OB expr_inner_attrs? expr_list? CB
    | OB expr_inner_attrs? expr SEMI expr CB
    | MOVE? closure_params closure_tail
    | blocky_expr
    | BREAK lifetime_or_expr?
    | CONTINUE Lifetime?
    | RETURN expr?;  // this is IMO a rustc bug, should be expr_no_struct

lit:
    TRUE
    | FALSE
    | BareIntLit
    | FullIntLit
    | ByteLit
    | ByteStringLit
    | FloatLit
    | CharLit
    | StringLit;

closure_params:
    OROR
    | OR closure_param_list? OR;

closure_param:
    pat (COL ty)?;

closure_param_list:
    closure_param (CO closure_param)* CO?;

closure_tail:
    rtype block
    | expr;

lifetime_or_expr:
    Lifetime
    | expr_no_struct;

fields:
    struct_update_base
    | field (CO field)* (CO struct_update_base | CO?);

struct_update_base:
    DOTDOT expr;  // this is IMO a bug in the grammar. should be or_expr or something.

field:
    ident  // struct field shorthand (field and local variable have the same name)
    | field_name COL expr;

field_name:
    ident
    | BareIntLit;  // Allowed for creating tuple struct values.


// --- Operators

post_expr:
    prim_expr
    | post_expr post_expr_tail;

post_expr_tail:
    QU
    | OB expr CB
    | DOT ident ((COCO ty_args)? OP expr_list? CP)?
    | DOT BareIntLit
    | OP expr_list? CP;

pre_expr:
    post_expr
    | expr_attrs pre_expr
    | MINUS pre_expr
    | NOT pre_expr
    | AND MUT? pre_expr
    | ANDAND MUT? pre_expr   // meaning `& & expr`
    | STAR pre_expr
    | BOX pre_expr
    | IN expr_no_struct block;  // placement new - possibly not the final syntax

cast_expr:
    pre_expr
    | cast_expr AS ty_sum
    | cast_expr COL ty_sum;  // experimental type ascription

mul_expr:
    cast_expr
    | mul_expr STAR cast_expr
    | mul_expr SLASH cast_expr
    | mul_expr PERCENT cast_expr;

add_expr:
    mul_expr
    | add_expr PLUS mul_expr
    | add_expr MINUS mul_expr;

shift_expr:
    add_expr
    | shift_expr SHL add_expr
    | shift_expr SHR add_expr;

bit_and_expr:
    shift_expr
    | bit_and_expr AND shift_expr;

bit_xor_expr:
    bit_and_expr
    | bit_xor_expr CARET bit_and_expr;

bit_or_expr:
    bit_xor_expr
    | bit_or_expr OR bit_xor_expr;

cmp_expr:
    bit_or_expr
    | bit_or_expr (EQEQ | NE | LT | LE | GT | GE) bit_or_expr;

and_expr:
    cmp_expr
    | and_expr ANDAND cmp_expr;

or_expr:
    and_expr
    | or_expr OROR and_expr;

range_expr:
    or_expr
    | or_expr DOTDOT or_expr?
    | DOTDOT or_expr?;

assign_expr:
    range_expr
    | range_expr (EQ | STAREQ | SLASHEQ | PERCENTEQ | PLUSEQ | MINUSEQ
                      | SHLEQ | SHREQ | ANDEQ | CARETEQ | OREQ ) assign_expr;


// --- Copy of the operator expression syntax but without structs

post_expr_no_struct:
    prim_expr_no_struct
    | post_expr_no_struct post_expr_tail;

pre_expr_no_struct:
    post_expr_no_struct
    | expr_attrs pre_expr_no_struct
    | MINUS pre_expr_no_struct
    | NOT pre_expr_no_struct
    | AND MUT? pre_expr_no_struct
    | ANDAND MUT? pre_expr_no_struct   // meaning `& & expr`
    | STAR pre_expr_no_struct
    | BOX pre_expr_no_struct;

cast_expr_no_struct:
    pre_expr_no_struct
    | cast_expr_no_struct AS ty_sum
    | cast_expr_no_struct COL ty_sum;  // experimental type ascription

mul_expr_no_struct:
    cast_expr_no_struct
    | mul_expr_no_struct STAR cast_expr_no_struct
    | mul_expr_no_struct SLASH cast_expr_no_struct
    | mul_expr_no_struct PERCENT cast_expr_no_struct;

add_expr_no_struct:
    mul_expr_no_struct
    | add_expr_no_struct PLUS mul_expr_no_struct
    | add_expr_no_struct MINUS mul_expr_no_struct;

shift_expr_no_struct:
    add_expr_no_struct
    | shift_expr_no_struct SHL add_expr_no_struct
    | shift_expr_no_struct SHR add_expr_no_struct;

bit_and_expr_no_struct:
    shift_expr_no_struct
    | bit_and_expr_no_struct AND shift_expr_no_struct;

bit_xor_expr_no_struct:
    bit_and_expr_no_struct
    | bit_xor_expr_no_struct CARET bit_and_expr_no_struct;

bit_or_expr_no_struct:
    bit_xor_expr_no_struct
    | bit_or_expr_no_struct OR bit_xor_expr_no_struct;

cmp_expr_no_struct:
    bit_or_expr_no_struct
    | bit_or_expr_no_struct (EQEQ | NE | LT | LE | GT | GT EQ) bit_or_expr_no_struct;

and_expr_no_struct:
    cmp_expr_no_struct
    | and_expr_no_struct ANDAND cmp_expr_no_struct;

or_expr_no_struct:
    and_expr_no_struct
    | or_expr_no_struct OROR and_expr_no_struct;

range_expr_no_struct:
    or_expr_no_struct
    | or_expr_no_struct DOTDOT or_expr_no_struct?
    | DOTDOT or_expr_no_struct?;

assign_expr_no_struct:
    range_expr_no_struct
    | range_expr_no_struct (EQ | STAREQ | SLASHEQ | PERCENTEQ | PLUSEQ | MINUSEQ
                                | SHLEQ | SHREQ | ANDEQ | CARETEQ | OREQ ) assign_expr_no_struct;


// === Tokens

// `auto`, `default`, and 'union' are identifiers, but in certain places
// they're specially recognized as keywords.
ident:
    Ident
    | AUTO
    | DEFAULT
    | UNION;

any_ident:
    ident
    | SELF
    | STATIC
    | SUPER;

